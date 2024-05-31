using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovementBlue : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolTarget = 0;

    void Start()
    {

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("Nema patrolnih ciljanih točaka.");
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
}
