using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovement : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public float stepLength = 2f;
    public float attackRange = 1f;
    public Transform player;
    public int health = 6;
    public int damageAmount = 1;  // Koliko štete goblin nanosi igraču
    public float damageInterval = 1f;  // Koliko često može nanijeti štetu igraču

    private float stepCounter;
    private int direction = 1;
    private float nextFlipTime;
    private float flipInterval = 4f;
    private bool isAttacking = false;
    private float lastDamageTime;  // Kada je goblin zadnji put nanio štetu

    void Start()
    {
        stepCounter = stepLength;
        nextFlipTime = Time.time + flipInterval;
        lastDamageTime = -damageInterval;  // Omogućuje trenutni napad na početku
    }

    void Update()
    {
        DetectAndAttackPlayer();
        if (!isAttacking)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        stepCounter -= speed * Time.deltaTime;

        if (Time.time >= nextFlipTime)
        {
            Flip();
            nextFlipTime += flipInterval;
        }
    }

    private void DetectAndAttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            Debug.Log("Attack the player!");
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
        // Provjera ako može nanijeti štetu
        if (Time.time >= lastDamageTime + damageInterval)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.Translate(directionToPlayer * speed * Time.deltaTime);

            // Promjena smjera ovisno o poziciji igrača
            if (player.position.x > transform.position.x && direction != 1)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && direction != -1)
            {
                Flip();
            }

            // Pronađi komponentu zdravlja igrača i nanesi štetu
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                lastDamageTime = Time.time;  // Postavi zadnji napad na sada
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took damage, remaining health: " + health);
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

    private void Flip()
    {
        direction *= -1;
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Ako je igrač unutar napada, nanesi mu štetu
        if (collision.CompareTag("Player"))
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
}