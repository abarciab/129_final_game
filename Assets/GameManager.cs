using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
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
    [SerializeField] public SpriteRenderer background;
    [SerializeField] List<SpriteRenderer> groundRend;
    [SerializeField] List<ListWrapper> p1reactions, p2reactions;

    [Header("player sprites")]
    [SerializeField] public List<Sprite> p1Idle;
    [SerializeField] public List<Sprite> p1Attack, p1Hurt, p2Idle, p2Attack, p2Hurt, swords, backgroundSprites, grounds;
    [HideInInspector] public bool fighting;

    int fightNum;
    bool started;

    private void Start()
    {
        started = false;
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
        player1UI.gameObject.SetActive(false);
        player2UI.gameObject.SetActive(false);
        foreach (var m in playerMoves) m.enabled = false;

        DialogueController.Chapter.Outcome outcome = victim == players[0] ? DialogueController.Chapter.Outcome.Wins : DialogueController.Chapter.Outcome.Loses;
        if (dialogue.StartDialogue(outcome)) return;
        SceneManager.LoadScene(0);
    }

    public void StartFight() {
        print("AHH START FIGHT");
        if (started) foreach (var m in playerMoves) m.canDash = true;

        if (started) fightNum += 1;
        started = true;
        if (fightNum == 3) return;
        background.sprite = backgroundSprites[fightNum];
        foreach (var g in groundRend) g.sprite = grounds[fightNum];
        SetUpPlayerSprites();

        foreach (var m in playerMoves) m.enabled = true;
        foreach (var p in players) {
            p.GetComponent<PlayerMovement>().stopped = false;
            p.FullHeal();
            p.gameObject.SetActive(true);
        }

        dialogue.gameObject.SetActive(false);
        player1UI.gameObject.SetActive(true);
        if (players.Count > 1) player2UI.gameObject.SetActive(true);
        fighting = true;
    }

    void SetUpPlayerSprites()
    {
        for (int i = 0; i < players.Count; i++) {
            bool p1 = i == 0;
            players[i].idleSprite = p1 ? p1Idle[fightNum] : p2Idle[fightNum];
            players[i].attackSprite = p1 ? p1Attack[fightNum] : p2Attack[fightNum];
            players[i].hurtSprite = p1 ? p1Hurt[fightNum] : p2Hurt[fightNum];
            players[i].reactions = p1 ? p1reactions[fightNum].reactionLines : p2reactions[fightNum].reactionLines;
            players[i].UpdateSprite();
        }
    }
}
