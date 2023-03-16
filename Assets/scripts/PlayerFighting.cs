using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFighting : MonoBehaviour
{
    public enum Stance { stab, slash};

    [SerializeField] Animator swordAnim;
    [SerializeField] float attackRadius, heavyAttackRadius,  normalAttackDmg, heavyAttackdmg, attackCooldown = 0.75f, missStunTime = 0.5f, blockStunTime = 0.5f, switchStunTime = 0.1f;
    [SerializeField] bool drawGizmos;
    [SerializeField] Transform attackPoint;

    [Header("Dash Attack")]
    public bool dashAttackEnabled;
    [SerializeField] float DAcheckRadius = 0.5f, extraDashTime = 1.5f, dashAttackStunTime = 0.2f;
    [SerializeField] Vector3 DAattackPoint;
    [SerializeField] bool showDAcircle;

    bool slashing, attacking, dashAttacking;
    PlayerFighting playerHitWhileDashing;
    float timeSinceAttack;
    [HideInInspector] public Stance stance;
    PlayerMovement moveScript;
    SpriteRenderer sRend;

    [Header("Sounds")]
    [SerializeField] int miss;
    [SerializeField] int hit, defend = 5;

    public void PressDashAttack(InputAction.CallbackContext ctx) {
        if (!enabled || !dashAttackEnabled || !ctx.started || !GameManager.instance.fighting) return;
        DashAttack();
    }


    public void SwitchToSlashAttack(InputAction.CallbackContext ctx)
    {
        if (!enabled || !ctx.started || stance == Stance.slash|| !GameManager.instance.fighting) return;
        Switch(Stance.slash);
    }

    public void SwitchToStabAttack(InputAction.CallbackContext ctx)
    {
        if (!enabled || !ctx.started || stance == Stance.stab || !GameManager.instance.fighting) return;
        Switch(Stance.stab);
    }


    public void PressAttack(InputAction.CallbackContext ctx)
    {
        if (enabled && !dashAttacking && ctx.started && GameManager.instance.fighting) StartAttack();
    }

    void DashAttack() {
        if (!moveScript.canDash) return;
        moveScript.Dash(extraDashTime);
        dashAttacking = true;
        playerHitWhileDashing = null;
    }


    void Switch(Stance type)
    {
        if (!GameManager.instance.fighting) return;
        stance = type;
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
        swordAnim.SetBool("StabDefend", stance == Stance.stab);
        swordAnim.SetBool("SlashDefend", stance == Stance.slash);

        if (dashAttacking) CheckForOppCollision();
        if (dashAttacking && !moveScript.dashing) EndDashAttack();
    }

    void EndDashAttack() {
        dashAttacking = false;
        if (playerHitWhileDashing == null) AudioManager.instance.PlaySound(miss, gameObject);
        StartCoroutine(StunPlayer(dashAttackStunTime));

    }

    void CheckForOppCollision() {
        if (playerHitWhileDashing != null) return;

        var hits = Physics2D.OverlapCircleAll(swordAnim.transform.position + DAattackPoint, DAcheckRadius);
        foreach (var h in hits) {
            var fighter = h.GetComponent<PlayerFighting>();
            if (fighter && fighter != this) DashHit(fighter);
        }
    }

    void DashHit(PlayerFighting victim) {
        playerHitWhileDashing = victim;

        if (stance == victim.stance || victim.GetComponent<PlayerMovement>().stopped) {
            GetBlocked();
            EndDashAttack();
            return;
        }

        HitVictim(victim.GetComponent<PlayerStats>());
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
            MissAttack();
            return;
        }
        var otherPlayer = victim.GetComponent<PlayerFighting>();
        if (!WillHit(otherPlayer)) {
            GetBlocked();
            return;
        }
        HitVictim(victim);
    }

    void MissAttack() {
        AudioManager.instance.PlaySound(miss, gameObject);
        StartCoroutine(StunPlayer(missStunTime));
    }

    void GetBlocked() {
        swordAnim.SetTrigger("Deflected");
        StartCoroutine(StunPlayer(blockStunTime));
        AudioManager.instance.PlaySound(defend, gameObject);
    }

    void HitVictim(PlayerStats victim) {
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

    bool WillHit(PlayerFighting otherPlayer)
    {
        var opponentStance = otherPlayer.stance;
        if (otherPlayer.GetComponent<PlayerMovement>().stopped) return true;
        if (opponentStance == Stance.stab && slashing) return true;
        if (opponentStance == Stance.slash && !slashing) return true;
        return false;
    }


    void StartAttack()
    {
        if (!moveScript || moveScript.stopped || attacking || timeSinceAttack < attackCooldown) return;
        
        attacking = true;
        if (stance == Stance.slash) slashAttack();
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
        Gizmos.DrawWireSphere(transform.position + attackPoint.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackPoint.position, heavyAttackRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(swordAnim.transform.position + DAattackPoint, DAcheckRadius);
    }

}
