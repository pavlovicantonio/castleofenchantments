using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;

    public GameObject gameOverScreen; // Povuci ovdje GameObject sa slikom "GAME OVER"
    public float resetDelay = 3f; // Vrijeme čekanja prije ponovnog pokretanja razine

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Osiguraj da je ekran skriven na početku
        }
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

    // Funkcija za ozdravljenje
    public void Heal(int amount)
    {
        currentHealth += amount;
        // Osiguravamo da trenutni HP ne prelazi maksimalan HP
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (isDead) return; // Sprječava višestruku aktivaciju smrti

        isDead = true;

        // Prikazivanje slike "GAME OVER"
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Resetiranje levela nakon kašnjenja
        Invoke(nameof(ResetLevel), resetDelay);
    }

    private void ResetLevel()
    {
        // Ponovno pokreće specifičnu razinu pod nazivom "Level1"
        SceneManager.LoadScene("Level1");
    }
}
