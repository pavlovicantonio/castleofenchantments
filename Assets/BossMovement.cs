using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float attackRange = 2f;
    public Animator animator;

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

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        Debug.Log("Attacking Player!");

    }
}
