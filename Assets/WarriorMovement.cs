using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WarriorMovement : MonoBehaviour
{
    public float speed = 2f;
    public float stepLength = 2f;
    public float attackRange = 1f;
    public Transform player;

    private float stepCounter;
    private int direction = 1;
    private float nextFlipTime;
    private float flipInterval = 4f;
    private bool isAttacking = false;

    void Start()
    {
        stepCounter = stepLength;
        nextFlipTime = Time.time + flipInterval;
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
        // Only move in step pattern if not attacking
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
        // Change this function to include actual attack logic
        // Move towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.Translate(directionToPlayer * speed * Time.deltaTime);

        // Optionally flip the goblin to face the player
        if (player.position.x > transform.position.x && direction != 1)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && direction != -1)
        {
            Flip();
        }
    }

    private void Flip()
    {
        direction *= -1;
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
