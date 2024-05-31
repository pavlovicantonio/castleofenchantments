using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovementBlack : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolTarget;

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
}

