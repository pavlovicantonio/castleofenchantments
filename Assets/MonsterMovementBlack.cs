using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovementBlack : MonoBehaviour, IDamageable
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolTarget;
    public int health = 6;

    void Start()
    {
        patrolTarget = 0;
    }

    void Update()
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
        Debug.Log("Damage taken: " + damage + ". Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("Monster is dead!");
        Destroy(gameObject);
    }

}
