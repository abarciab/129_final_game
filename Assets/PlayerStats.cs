using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float _health, iFrameTime;
    public float health { get { return _health; } set { SetHealth(value); } }
    public float maxHealth;
    bool invincible;
    [SerializeField] int numFlashing = 25;
    public SpriteRenderer sRend;

    [HideInInspector] public Sprite idleSprite, attackSprite, hurtSprite;

    [Header("Sounds")]
    [SerializeField] int hurt;

    private void Start()
    {
        GameManager.instance.AddPlayer(this);
        sRend.sprite = idleSprite;
        if (idleSprite.name.Contains("2")) sRend.transform.Rotate(0, 180, 0);
        _health = maxHealth;
    }

    void SetHealth(float change)
    {
        if (change < 0) Damage(change);
        else Heal(change);
    }

    public void Damage(float change)
    {
        if (invincible) return;
        AudioManager.instance.PlaySound(hurt, gameObject);
        _health += change;

        if (health <= 0) Die();
        else StartCoroutine(Invincible());
    }

    void Die()
    {
        GameManager.instance.PlayerDie(this);
    }

    IEnumerator Invincible()
    {
        invincible = true;
        float time = 0, interval = iFrameTime/numFlashing;
        sRend.sprite = hurtSprite;

        while (time < iFrameTime) {
            if (sRend.color != Color.black) sRend.color = Color.black;
            else sRend.color = Color.white;

            time += interval;
            yield return new WaitForSeconds(interval);
        }

        sRend.color = Color.white;
        sRend.sprite = idleSprite;
        invincible = false;
    }

    public void Heal(float change)
    {
        _health += change;
    }
}
