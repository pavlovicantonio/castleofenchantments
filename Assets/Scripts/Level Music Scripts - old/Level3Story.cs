using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Level3Story : MonoBehaviour
{
    public TMP_Text textComponent; // Referenca na TextMeshPro komponentu
    public Image imageObject; // Referenca na Image komponentu koju želimo sakriti
    private Queue<string> sentences; // Redoslijed rečenica
    private Coroutine typingCoroutine;

    void Start()
    {
        sentences = new Queue<string>();

        if (textComponent == null)
        {
            textComponent = GetComponent<TMP_Text>();
            if (textComponent == null)
            {
                Debug.LogError("TextMeshPro component is not assigned and cannot be found on the GameObject.");
                return;
            }
        }

        // Dodavanje rečenica u queue
        sentences.Enqueue("Congratulations on completing the second level! You have overcome the threats in forest more easily than expected. ...");
        sentences.Enqueue("... But that’s not all... Ahead of you lie dangerous mountains with steep cliffs...");
        sentences.Enqueue("... Beware of the vicious bandits roaming these mountains.... be careful! ...");
        sentences.Enqueue("... Good luck, and don’t forget, time is running out!");

        // Prikaz prve rečenice
        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartFadeOut(); // Pokreće fade out nakon zadnje rečenice
            return;
        }

        string sentence = sentences.Dequeue();
        StartTypingSentence(sentence);
    }

    private void StartTypingSentence(string sentence)
    {
        // Zaustavi prethodnu korutinu ako je aktivna
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Resetira tekst prije ispisivanja nove rečenice
        textComponent.text = "";

        // Pokreće korutinu za ispisivanje rečenice
        typingCoroutine = StartCoroutine(TypeSentenceCoroutine(sentence));
    }

    private IEnumerator TypeSentenceCoroutine(string sentence)
    {
        // Ispisuje tekst znak po znak
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.05f); // Brzina ispisivanja
        }

        // Prikazuje sljedeću rečenicu nakon završetka ispisivanja
        ShowNextSentence();
    }

    private void StartFadeOut()
    {
        if (imageObject != null)
        {
            StartCoroutine(FadeOutImage());
        }
    }

    private IEnumerator FadeOutImage()
    {
        Color imageColor = imageObject.color;
        float fadeDuration = 2.0f; // Trajanje efekta fade out
        float startAlpha = imageColor.a;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            imageColor.a = Mathf.Lerp(startAlpha, 0, normalizedTime); // Postepeno smanjuje alfa vrijednost
            imageObject.color = imageColor;
            yield return null;
        }

        // Postavljanje alfa vrijednosti na 0 kako bi se potpuno sakrila slika
        imageColor.a = 0;
        imageObject.color = imageColor;

        // Deaktiviranje Image objekta nakon završetka fade out efekta
        imageObject.gameObject.SetActive(false);
        imageObject.enabled = false;
    }
}
