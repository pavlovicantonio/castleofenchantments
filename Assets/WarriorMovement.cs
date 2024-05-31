using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WarriorMovement : MonoBehaviour
{
    public float speed = 2f;
    public float stepLength = 2f;
    public float attackRange = 1f;
    public Transform player;
    public LayerMask playerLayer;

    private float stepCounter;
    private int direction = 1;
    private float nextFlipTime;
    private float flipInterval = 4f;

    void Start()
    {
        stepCounter = stepLength;
        nextFlipTime = Time.time + flipInterval;
    }

    void Update()
    {
        Move();
        DetectAndAttackPlayer();
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

        }
    }

    private void Flip()
    {
        direction *= -1;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
