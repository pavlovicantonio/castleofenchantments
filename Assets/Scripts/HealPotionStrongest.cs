using UnityEngine;

public class HealPotionStrongest : MonoBehaviour
{
    public int healAmount = 5; // Količina HP-a koju će potion obnoviti

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Provjerava je li objekt koji je ušao u koliziju igrač
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Poziva funkciju za ozdravljenje
            playerHealth.Heal(healAmount);

            // Uništava potion nakon što ga igrač pokupi
            Destroy(gameObject);
        }
    }
}
