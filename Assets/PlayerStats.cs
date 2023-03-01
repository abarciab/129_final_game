using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float _health, iFrameTime;
    public float health { get { return _health; } set { SetHealth(value); } }
    public float maxHealth;
    [HideInInspector] public bool invincible;
    [SerializeField] int numFlashing = 25;
    public SpriteRenderer sRend;
    [SerializeField] Vector3 knockBack;
    [SerializeField] TextMeshProUGUI reactionText;
    [HideInInspector] public List<string> reactions;


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

    public void UpdateSprite()
    {
        sRend.sprite = idleSprite;
    }

    public void FullHeal() {
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

        var kb = knockBack;
        kb.x = kb.x * (Mathf.Abs(transform.eulerAngles.y) > 01f ? -1 : 1);
        transform.position += kb;

        reactionText.gameObject.SetActive(false);
        reactionText.text = reactions[Random.Range(0, reactions.Count)];
        reactionText.gameObject.SetActive(true);
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
