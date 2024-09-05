using UnityEngine;
using System.Collections; // Dodano za IEnumerator
using System.Collections.Generic; // Dodano za Queue<>
using TMPro; // Za korištenje TextMeshPro

public class BossAlertLevel3 : MonoBehaviour
{
    // Povuci samo onaj Image objekt (Canvas) koji se treba prikazivati kad igrač dođe do točke
    public GameObject imageObjectToActivate;
    public TMP_Text textComponent;  // Referenca na TextMeshPro komponentu

    private Queue<string> sentences; // Queue za rečenice koje će se ispisivati
    private Coroutine typingCoroutine; // Korutina za ispisivanje teksta

    private void Start()
    {
        sentences = new Queue<string>();

        if (imageObjectToActivate != null)
        {
            imageObjectToActivate.SetActive(false); // Sakrij Image objekt na početku
        }

        // Dodavanje rečenica u queue
        sentences.Enqueue("Congratulations to the brave player for battling through and completing the third level...");
        sentences.Enqueue("... But don’t get too excited because, as you already know, another boss awaits you at the end of the level...");
        sentences.Enqueue("... This time, it’s Mushroomhead, but don’t be fooled by his height… he’s more dangerous than he looks ...");
        sentences.Enqueue("... Be careful, as the Boss is stronger than any threats you’ve faced so far!...");
        sentences.Enqueue("Good Luck!"); // Zadnja rečenica
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Provjeri je li igrač ušao u trigger zonu
        if (collision.CompareTag("Player"))
        {
            // Prikaži Image objekt
            if (imageObjectToActivate != null)
            {
                imageObjectToActivate.SetActive(true);
            }

            // Pokreni ispisivanje teksta
            ShowNextSentence();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Kad igrač izađe, sakrij Image objekt i zaustavi ispisivanje
        if (collision.CompareTag("Player"))
        {
            if (imageObjectToActivate != null)
            {
                imageObjectToActivate.SetActive(false);
            }

            // Resetiraj tekst kad igrač izađe
            if (textComponent != null)
            {
                textComponent.text = "";
            }
        }
    }

    private void ShowNextSentence()
    {
        if (sentences.Count == 0)
        {
            return; // Ako nema više rečenica, završava se
        }

        string sentence = sentences.Dequeue(); // Uzmi sljedeću rečenicu iz queuea
        StartTypingSentence(sentence);
    }

    private void StartTypingSentence(string sentence)
    {
        // Zaustavi prethodnu korutinu ako je aktivna
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Resetiraj tekst prije ispisivanja nove rečenice
        if (textComponent != null)
        {
            textComponent.text = "";
        }

        // Pokreće korutinu za ispisivanje rečenice
        typingCoroutine = StartCoroutine(TypeSentenceCoroutine(sentence));
    }

    private IEnumerator TypeSentenceCoroutine(string sentence)
    {
        // Ispisuje tekst znak po znak
        foreach (char letter in sentence.ToCharArray())
        {
            if (textComponent != null)
            {
                textComponent.text += letter;
            }
            yield return new WaitForSeconds(0.05f); // Brzina ispisivanja
        }

        // Nakon završetka ispisivanja rečenice, prikazuje sljedeću rečenicu
        ShowNextSentence();
    }
}