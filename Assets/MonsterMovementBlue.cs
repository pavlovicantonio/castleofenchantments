using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovementBlue : MonoBehaviour, IDamageable
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolTarget = 0;
    public int health = 6;

    void Start()
    {
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points set.");
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        MoveToNextPatrolPoint();
    }

    void MoveToNextPatrolPoint()
    {
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
        Debug.Log($"Damage taken: {damage}. Current health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        {
            Debug.Log("Monster is dead!");
            Destroy(gameObject);
        }

    }
}
