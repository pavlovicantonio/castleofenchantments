using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject swordCollider;
    public float attackCooldown = 0.5f;
    private float lastAttackTime = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        swordCollider.SetActive(true);
        Invoke("DeactivateSwordCollider", 0.1f);
    }

    void DeactivateSwordCollider()
    {
        swordCollider.SetActive(false);
    }
}
