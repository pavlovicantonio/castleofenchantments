using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MonsterHealth : MonoBehaviour
{
    public int health = 2;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " is dead.");
        Destroy(gameObject);
    }

}
