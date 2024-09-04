using UnityEngine;

public class BossAlertLevel1 : MonoBehaviour
{
    // Povuci samo onaj Image objekt (Canvas) koji se treba prikazivati kad igrač dođe do točke
    public GameObject imageObjectToActivate;

    private void Start()
    {
        // Sakrij Image objekt na početku
        if (imageObjectToActivate != null)
        {
            imageObjectToActivate.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Provjeri je li igrač ušao u trigger zonu
        if (collision.CompareTag("Player"))
        {
            // Prikaži samo željeni Image objekt
            if (imageObjectToActivate != null)
            {
                imageObjectToActivate.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Kad igrač izađe, sakrij Image objekt
        if (collision.CompareTag("Player"))
        {
            if (imageObjectToActivate != null)
            {
                imageObjectToActivate.SetActive(false);
            }
        }
    }
}
