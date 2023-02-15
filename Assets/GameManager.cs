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

    public void AddPlayer(PlayerStats newPlayer)
    {
        players.Add(newPlayer);
        bool p1 = players.Count == 1;
        var UI =  p1 ? player1UI : player2UI;
        UI.player = newPlayer;
        UI.gameObject.SetActive(true);

        newPlayer.idleSprite = p1 ? p1Idle : p2Idle;
        newPlayer.attackSprite = p1 ? p1Attack : p2Attack;
        newPlayer.hurtSprite = p1 ? p1Hurt : p2Hurt;

        if (players.Count == 2) GetComponent<PlayerInputManager>().DisableJoining();
    }

    public Vector3 otherPlayerPosition(PlayerStats player)
    {
        foreach (var p in players) if (p != player) return p.transform.position;
        return player.transform.position;
    }

    public void PlayerDie(PlayerStats victim)
    {
        
        SceneManager.LoadScene(0);
        players.Remove(victim);
        Destroy(victim.gameObject);
    }
}
