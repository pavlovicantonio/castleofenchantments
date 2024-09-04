using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Funkcija za primanje štete
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Nova funkcija za ozdravljenje
    public void Heal(int amount)
    {
        currentHealth += amount;
        // Osiguravamo da trenutni HP ne prelazi maksimalan HP
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    private void Die()
    {
        // Možete dodati dodatne efekte smrti ovdje
        Destroy(gameObject);
    }
}
