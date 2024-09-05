using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public float stepLength = 3f;
    public float attackRange = 1.5f;
    public Transform player;

    [SerializeField] float health, maxHealth = 10f; // Boss ima više zdravlja
    public int damageAmount = 2;  // Boss nanosi više štete
    public float damageInterval = 1.5f;  // Boss napada malo sporije

    private float stepCounter;
    private int direction = 1;
    private float nextFlipTime;
    private float flipInterval = 3f;
    private bool isAttacking = false;
    private float lastDamageTime;

    [SerializeField] FloatingHealthBar healthBar;  // Zdravstvena traka za bossa

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        stepCounter = stepLength;
        nextFlipTime = Time.time + flipInterval;
        lastDamageTime = -damageInterval;
        health = maxHealth; // Postavi početno zdravlje
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    void Update()
    {
        if (!isAttacking)
        {
            Move();
        }

        // Održavaj zdravstvenu traku stabilnom
        if (healthBar != null)
        {
            healthBar.transform.rotation = Quaternion.identity;
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        stepCounter -= speed * Time.deltaTime;

        if (Time.time >= nextFlipTime)
        {
            direction *= -1; // Promijeni smjer
            Flip();          // Okreni sprite
            nextFlipTime += flipInterval;
        }
    }

    private void Flip()
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took damage, remaining health: " + health);
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " is dead.");
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                var healthComponent = collision.GetComponent<PlayerHealth>();
                if (healthComponent != null)
                {
                    healthComponent.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var healthComponent = collision.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}