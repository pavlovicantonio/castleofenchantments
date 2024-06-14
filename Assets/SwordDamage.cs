using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwordDamage : MonoBehaviour
{
    public int damage = 2;

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}
