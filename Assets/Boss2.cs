using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour, IDamageable
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float attackRange = 2f;
    public int health = 6;

    private Transform playerTransform;
    private Vector2 startPosition;
    private Vector2 moveDirection = Vector2.right;
    private float moveTimer;

    void Start()
    {
        startPosition = transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (health <= 0) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (moveTimer >= patrolDistance)
        {
            moveDirection *= -1;
            moveTimer = 0;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        moveTimer += Time.deltaTime;
    }

    void AttackPlayer()
    {
        Debug.Log("Attacking Player!");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss is dead!");
        Destroy(gameObject);
    }
}
