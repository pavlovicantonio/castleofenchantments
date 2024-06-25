using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject swordCollider;
    public float attackCooldown = 0.5f;
    private float lastAttackTime = 0;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        AdjustSwordCollider();
        swordCollider.SetActive(true);
        Invoke("DeactivateSwordCollider", 0.1f);
    }

    void DeactivateSwordCollider()
    {
        swordCollider.SetActive(false);
    }

    void AdjustSwordCollider()
    {
        Vector3 newPosition = swordCollider.transform.localPosition;
        Vector3 newScale = swordCollider.transform.localScale;

        if (spriteRenderer.flipX)
        {
            newPosition.x = -Mathf.Abs(newPosition.x); // Inverzija x pozicije
            newScale.x = -Mathf.Abs(newScale.x); // Inverzija x skale
        }
        else
        {
            newPosition.x = Mathf.Abs(newPosition.x); // Postavi na normalnu x poziciju
            newScale.x = Mathf.Abs(newScale.x); // Postavi na normalnu x skalu
        }

        swordCollider.transform.localPosition = newPosition;
        swordCollider.transform.localScale = newScale;
    }
}
