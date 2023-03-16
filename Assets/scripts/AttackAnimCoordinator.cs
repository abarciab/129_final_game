using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimCoordinator : MonoBehaviour
{
    [SerializeField] PlayerFighting player;

    public void CompleteAnim()
    {
        player.CompleteAttack();
    }
}
