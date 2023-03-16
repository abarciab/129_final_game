using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() { instance = this; }

    [System.Serializable]
    public class ListWrapper
    {
        public List<string> reactionLines;
    }

    [SerializeField] Color player1, player2;
    [SerializeField] PlayerUICoordinator player1UI, player2UI;
    [SerializeField] DialogueController dialogue;

    List<PlayerStats> players = new List<PlayerStats>();
    List<PlayerMovement> playerMoves = new List<PlayerMovement>();
    [SerializeField] List<SpriteRenderer> groundRend;
    [SerializeField] List<ListWrapper> p1reactions, p2reactions;
    [SerializeField] TextMeshProUGUI tutorialPrompt;

    [Header("player sprites")]
    [SerializeField] public List<Sprite> p1Idle;
    [SerializeField] public List<Sprite> p1Attack, p1Hurt, p2Idle, p2Attack, p2Hurt, swords, grounds;
    [HideInInspector] public bool fighting;
    [SerializeField] List<float> playerSpeeds, playerJumps;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resumeButton;
    [HideInInspector] public bool paused;

    [Header("Music")]
    public AudioSource musicSource;
    [SerializeField] AudioClip music2, music3;
    [SerializeField] float volume2, volume3;

    int fightNum;
    bool started;

    private void Start()
    {
        started = false;
        UnpauseGame();
    }

    public void PauseGame() {
        if (paused || !fighting) return; 

        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);

        foreach (var p in players) {
            p.enabled = false;
            p.GetComponent<PlayerMovement>().enabled = false;
            p.GetComponent<PlayerFighting>().enabled = false;
        }
        paused = true;
    }

    public void UnpauseGame() {
        pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        foreach (var p in players) {
            p.enabled = false;
            p.GetComponent<PlayerMovement>().enabled = fighting;
            p.GetComponent<PlayerFighting>().enabled = fighting;
        }
        paused = false;
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void NextDialogue (PlayerStats player) {
        if (fighting || players.Count < 1) return;
        dialogue.NextOrSkip(player != players[0]);
    }

    public void AddPlayer(PlayerStats newPlayer)
    {
        players.Add(newPlayer);
        playerMoves.Add(newPlayer.GetComponent<PlayerMovement>());

        bool p1 = players.Count == 1;
        var UI =  p1 ? player1UI : player2UI;
        UI.player = newPlayer;
        UI.gameObject.SetActive(fighting);
        newPlayer.reactions = p1 ? p1reactions[0].reactionLines : p2reactions[0].reactionLines;

        newPlayer.idleSprite = p1 ? p1Idle[fightNum] : p2Idle[fightNum];
        newPlayer.attackSprite = p1 ? p1Attack[fightNum] : p2Attack[fightNum];
        newPlayer.hurtSprite = p1 ? p1Hurt[fightNum] : p2Hurt[fightNum];

        newPlayer.GetComponent<PlayerMovement>().stopped = !fighting;
        if (players.Count == 2) GetComponent<PlayerInputManager>().DisableJoining();
        if (!fighting && !dialogue.gameObject.activeInHierarchy) dialogue.StartDialogue();
    }

    public Vector3 otherPlayerPosition(PlayerStats player)
    {
        foreach (var p in players) if (p != player) return p.transform.position;
        return player.transform.position;
    }

    public void PlayerDie(PlayerStats victim)
    {
        fighting = false;

        player1UI.gameObject.SetActive(false);
        player2UI.gameObject.SetActive(false);
        foreach (var m in playerMoves) m.enabled = false;

        DialogueController.Chapter.Outcome outcome = victim == players[0] ? DialogueController.Chapter.Outcome.Wins : DialogueController.Chapter.Outcome.Loses;
        if (dialogue.StartDialogue(outcome)) return;
        SceneManager.LoadScene(0);
    }

    public void StartFight() {
        if (started) fightNum += 1;
        started = true;

        if (fightNum == 3) StartCoroutine(EndGame());
        if (fightNum >= 3) return;

        foreach (var g in groundRend) g.sprite = grounds[fightNum];

        foreach (var m in playerMoves) m.enabled = true;
        foreach (var p in players) {
            p.GetComponent<PlayerMovement>().stopped = false;
            p.FullHeal();
            p.gameObject.SetActive(true);
            if (fightNum == 2) p.GetComponent<PlayerFighting>().dashAttackEnabled = true;
        }
        if (fightNum == 1) {
            foreach (var m in playerMoves) {
                m.canDash = true;
                m.speed = playerSpeeds[0];
                m.jumpForce = playerJumps[0];
            }
            tutorialPrompt.text = "Press X To dash";
        }
        if (fightNum == 2) {
            foreach (var m in playerMoves) {
                m.speed = playerSpeeds[1];
                m.jumpForce = playerJumps[1];
            }
            tutorialPrompt.text = "Press Y To charge";
        }

        if (fightNum != 0) {
            tutorialPrompt.gameObject.SetActive(false);
            tutorialPrompt.gameObject.SetActive(true);
        }

        dialogue.gameObject.SetActive(false);
        player1UI.gameObject.SetActive(true);
        if (players.Count > 1) player2UI.gameObject.SetActive(true);
        fighting = true;
    }

    public IEnumerator StartMusic() {
        AudioClip song = fightNum == 0 ? music2 : music3;
        float targetVolume = fightNum == 0 ? volume2 : volume3;

        musicSource.clip = song;
        musicSource.Play();
        while (Mathf.Abs(musicSource.volume - targetVolume) > 0.001f) {
            musicSource.volume = Mathf.Lerp(musicSource.volume, targetVolume, 0.025f);
            yield return new WaitForEndOfFrame();
        }
        musicSource.volume = targetVolume;
    }

    IEnumerator EndGame() {
        yield return new WaitForSeconds(1);
        var wipe = dialogue.blackWipe;
        wipe.transform.localPosition = Vector3.right * dialogue.wipeLeftRightPos.y;
        while (wipe.transform.localPosition.x > 10) {
            wipe.transform.localPosition = Vector3.Lerp(wipe.transform.localPosition, Vector3.zero, 0.025f);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0);
    }

    public void SetUpPlayerSprites()
    {
        fightNum += 1;
        for (int i = 0; i < players.Count; i++) {
            bool p1 = i == 0;
            players[i].idleSprite = p1 ? p1Idle[fightNum] : p2Idle[fightNum];
            players[i].attackSprite = p1 ? p1Attack[fightNum] : p2Attack[fightNum];
            players[i].hurtSprite = p1 ? p1Hurt[fightNum] : p2Hurt[fightNum];
            players[i].reactions = p1 ? p1reactions[fightNum].reactionLines : p2reactions[fightNum].reactionLines;
            players[i].swordSprite = swords[fightNum];
            players[i].UpdateSprite();
        }
        fightNum -= 1;
    }
}
