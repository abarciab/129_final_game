using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() { instance = this; }

    [SerializeField] Color player1, player2;
    [SerializeField] PlayerUICoordinator player1UI, player2UI;
    [SerializeField] DialogueController dialogue;

    List<PlayerStats> players = new List<PlayerStats>();

    [Header("player sprites")]
    [SerializeField] Sprite p1Idle;
    [SerializeField] Sprite p1Attack, p1Hurt, p2Idle, p2Attack, p2Hurt;
    bool fighting;  

    public void AddPlayer(PlayerStats newPlayer)
    {
        players.Add(newPlayer);
        bool p1 = players.Count == 1;
        var UI =  p1 ? player1UI : player2UI;
        UI.player = newPlayer;
        UI.gameObject.SetActive(fighting);

        newPlayer.idleSprite = p1 ? p1Idle : p2Idle;
        newPlayer.attackSprite = p1 ? p1Attack : p2Attack;
        newPlayer.hurtSprite = p1 ? p1Hurt : p2Hurt;

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
        DialogueController.Chapter.Outcome outcome = victim == players[0] ? DialogueController.Chapter.Outcome.Wins : DialogueController.Chapter.Outcome.Loses;

        player1UI.gameObject.SetActive(false);
        player2UI.gameObject.SetActive(false);
        foreach (var p in players) p.gameObject.SetActive(false);

        if (dialogue.StartDialogue(outcome)) return;
        
        SceneManager.LoadScene(0);
    }

    public void StartFight() {
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
}
