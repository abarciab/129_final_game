using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFighting : MonoBehaviour
{
    [SerializeField] Animator swordAnim;
    [SerializeField] float attackRadius, heavyAttackRadius,  normalAttackDmg, heavyAttackdmg, attackCooldown = 0.75f, missStunTime = 0.5f, blockStunTime = 0.5f, switchStunTime = 0.1f;
    [SerializeField] bool drawGizmos;
    [SerializeField] Transform attackPoint;
    bool slashing, attacking;
    float timeSinceAttack;
    [HideInInspector] public bool stabDefend;
    PlayerMovement moveScript;
    SpriteRenderer sRend;

    [Header("Sounds")]
    [SerializeField] int miss;
    [SerializeField] int hit, defend = 5;

    public void PressNormalAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || !stabDefend) return;

        Switch(1);
    }

    public void PressHeavyAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || stabDefend) return;

        Switch(0);
    }

    public void PressStabDefend(InputAction.CallbackContext ctx)
    {
        return;
        if (ctx.started) stabDefend = true;
    }

    public void PressSlashDefend(InputAction.CallbackContext ctx)
    {
        return;
        if (ctx.started) stabDefend = false;
    }

    public void PressAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started) StartAttack();
    }

    void Switch(int type)
    {
        stabDefend = type == 1 ? false : true;
        StartCoroutine(StunPlayer(switchStunTime));
        AudioManager.instance.PlaySound(6, gameObject);
    }

    private void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
        sRend = GetComponent<PlayerStats>().sRend;
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        swordAnim.SetBool("Stunned", moveScript.stopped);
        swordAnim.SetBool("StabDefend", stabDefend);
        swordAnim.SetBool("SlashDefend", !stabDefend);
    }

    public void CompleteAttack()
    {
        moveScript.stopped = false;
        timeSinceAttack = 0;
        attacking = false;
        var colliders = Physics2D.OverlapCircleAll(attackPoint.transform.position, slashing ? heavyAttackRadius : attackRadius);

        PlayerStats victim = null;
        foreach (var c in colliders) if (c.GetComponent<PlayerStats>() && c.gameObject != gameObject) victim = c.GetComponent<PlayerStats>();

        

        if (!victim) {
            AudioManager.instance.PlaySound(miss, gameObject);
            StartCoroutine(StunPlayer(missStunTime));
            return;
        }

        var fighting = victim.GetComponent<PlayerFighting>();
        if (!WillHit(fighting)) {
            StartCoroutine(StunPlayer(blockStunTime));
            AudioManager.instance.PlaySound(defend, gameObject);
            return;
        }

        sRend.sprite = GetComponent<PlayerStats>().idleSprite;
        AudioManager.instance.PlaySound(hit, gameObject);
        victim.Damage(slashing ? -heavyAttackdmg : -normalAttackDmg);
    }

    IEnumerator StunPlayer(float stunTime)
    {
        moveScript.stopped = true;
        var srend = sRend;

        yield return new WaitForSeconds(stunTime);
        
        moveScript.stopped = false;
        sRend.sprite = GetComponent<PlayerStats>().idleSprite;
    }

    bool WillHit(PlayerFighting fighting)
    {
        var _stabDefend = fighting.stabDefend;
        if (fighting.GetComponent<PlayerMovement>().stopped) return true;
        if (_stabDefend && slashing) return true;
        if (!_stabDefend && !slashing) return true;
        return false;
    }


    void StartAttack()
    {
        if (!moveScript || moveScript.stopped || attacking || timeSinceAttack < attackCooldown) return;
        int attackType = stabDefend ? 0 : 1;

        attacking = true;
        if (attackType == 1) slashAttack();
        else NormalAttack();

        moveScript.stopped = true;
        sRend.sprite = GetComponent<PlayerStats>().attackSprite;
    }

    void slashAttack()
    {
        slashing = true;
        swordAnim.SetTrigger("HeavyAttack");
    }

    void NormalAttack()
    {
        slashing = false;
        swordAnim.SetTrigger("Attack");
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, heavyAttackRadius);
    }

}
