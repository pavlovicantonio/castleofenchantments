using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorMovement : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public float stepLength = 2f;
    public float attackRange = 1f;
    public Transform player;

    [SerializeField] float health, maxHealth = 6f;
    public int damageAmount = 1;
    public float damageInterval = 1f;

    private float stepCounter;
    private int direction = 1;
    private float nextFlipTime;
    private float flipInterval = 4f;
    private bool isAttacking = false;
    private float lastDamageTime;

    [SerializeField] FloatingHealthBar healthBar;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        stepCounter = stepLength;
        nextFlipTime = Time.time + flipInterval;
        lastDamageTime = -damageInterval;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    void Update()
    {
        if (!isAttacking)
        {
            Move();
        }

        // Keep the health bar's rotation fixed
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
            direction *= -1; // Change direction
            Flip();          // Update the sprite orientation
            nextFlipTime += flipInterval;
        }
    }

    private void Flip()
    {
        Debug.Log("Flipping sprite");
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
