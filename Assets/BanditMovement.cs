using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMovement : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public float detectionRange = 4f;
    public float stepLength = 2f;
    public Transform player;
    public int health = 6;

    private float stepCounter;
    private int direction = 1;
    private bool isAttacking = false;

    void Start()
    {
        stepCounter = stepLength;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (!isAttacking)
        {
            if (distanceToPlayer > detectionRange)
            {
                Patrol();
            }
            else
            {
                ApproachPlayer();
            }
        }

        if (distanceToPlayer < detectionRange / 2)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    private void Patrol()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        stepCounter -= Time.deltaTime;

        if (stepCounter <= 0)
        {
            direction *= -1;
            Flip();
            stepCounter = stepLength;
        }
    }

    private void ApproachPlayer()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, step);

        // Okretanje Bandita prema igraču
        if (player.position.x > transform.position.x && direction < 0)
        {
            direction = 1;
            Flip();
        }
        else if (player.position.x < transform.position.x && direction > 0)
        {
            direction = -1;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
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

    public void Die()
    {
        Debug.Log(gameObject.name + " is dead.");
        Destroy(gameObject);
    }
}
