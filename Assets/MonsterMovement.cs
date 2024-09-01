using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour, IDamageable
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolTarget;
    //public int health = 6;
    [SerializeField] float health, maxHealth = 6f;
    public int damageAmount = 1;  // Damage dealt per interval
    public float damageInterval = 1f;  // Time between damage ticks

    private float lastDamageTime;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        patrolTarget = 0;
        lastDamageTime = -damageInterval;  // Ensures immediate damage on first contact
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    void Update()
    {
        MoveToNextPatrolPoint();
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[patrolTarget];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            patrolTarget = (patrolTarget + 1) % patrolPoints.Length;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log(gameObject.name + " took damage, remaining health: " + health);
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
            // Deal damage at regular intervals
            if (Time.time >= lastDamageTime + damageInterval)
            {
                var healthComponent = collision.GetComponent<PlayerHealth>();
                if (healthComponent != null)
                {
                    healthComponent.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;  // Reset damage timer
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Optionally, deal immediate damage upon first contact
        if (collision.tag == "Player")
        {
            var healthComponent = collision.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageAmount);
                lastDamageTime = Time.time;  // Reset damage timer
            }
        }
    }
}
