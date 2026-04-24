using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public int currentHealth;

    public float deathAnimDuration = 1f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isInvincible = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        UIManager.instance?.UpdateLives(currentHealth);
    }

    public void Damage()
    {
        if (isInvincible || isDead) return;

        if (currentHealth > 1)
            StartCoroutine(ReceibeDamage());
        else
            StartCoroutine(Die());
    }

    public void InstantDeath()
    {
        if (isDead) return;
        StartCoroutine(Die());
    }

    IEnumerator ReceibeDamage()
    {
        isInvincible = true;
        Physics2D.IgnoreLayerCollision(3, 6, true);
        currentHealth--;
        UIManager.instance?.UpdateLives(currentHealth);
        animator.SetBool("isDamage", true);
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreLayerCollision(3, 6, false);
        animator.SetBool("isDamage", false);
        isInvincible = false;
    }

    IEnumerator Die()
    {
        isDead = true;
        GetComponent<NewInput>().enabled = false;
        GetComponent<PlayerMovement1>().enabled = false;
        rb.velocity = Vector2.zero;

        animator.SetBool("dieAnim", true);
        UIManager.instance?.UpdateLives(0);

        yield return new WaitForSeconds(deathAnimDuration);

        UIManager.instance?.ShowGameOver();
    }
}
