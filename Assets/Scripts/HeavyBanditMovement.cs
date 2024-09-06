using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBanditMovement : MonoBehaviour, IDamageable
{
    public float speed = 1f; // Teži bandit je sporiji
    public float stepLength = 2f; // Možda dulji koraci
    public float attackRange = 1.5f; // Veæi napadni raspon zbog velièine bandita
    public Transform player;

    [SerializeField] float health, maxHealth = 10f; // Veæe zdravlje
    public int damageAmount = 2; // Jaèi napadi
    public float damageInterval = 2f; // Duži interval izmeðu napada

    private float stepCounter;
    private int direction = 1;
    private float nextFlipTime;
    private float flipInterval = 2f; // Sporije promjene smjera
    private bool isAttacking = false;
    private float lastDamageTime;

    [SerializeField] FloatingHealthBar healthBar;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Dohvati SpriteRenderer komponentu
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        stepCounter = stepLength;
        nextFlipTime = Time.time + flipInterval;
        lastDamageTime = -damageInterval;
        health = maxHealth; // Inicijaliziraj zdravlje
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    void Update()
    {
        DetectAndAttackPlayer(); // Detekcija i napad
        if (!isAttacking)
        {
            Move(); // Ako ne napada, kreæe se
        }

        // Drži rotaciju health bara fiksiranom
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
            Flip();          // Ažuriraj orijentaciju sprita
            nextFlipTime += flipInterval;
        }
    }

    private void Flip()
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = false; // Gleda desno
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true; // Gleda lijevo
        }
    }

    private void DetectAndAttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            Debug.Log("Heavy Bandit attacks the player!");
            isAttacking = true;
            AttackPlayer();
        }
        else
        {
            isAttacking = false;
        }
    }

    private void AttackPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.Translate(directionToPlayer * speed * Time.deltaTime);

        if (player.position.x > transform.position.x && direction != 1)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && direction != -1)
        {
            Flip();
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
