using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICoordinator : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [HideInInspector] public PlayerStats player;

    private void Update()
    {
        if (!player) return;

        hpBar.value = player.health / player.maxHealth;
    }

}
