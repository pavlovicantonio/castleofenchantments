using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Level1Story2 : MonoBehaviour
{
    public TMP_Text textComponent; // Reference to the TextMeshPro component
    public Image imageObject; // Reference to the Image component to hide
    private Queue<string> sentences; // Queue of sentences
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

        // Adding sentences to the queue
        sentences.Enqueue("Congratulations to the brave player for battling through and completing the first level ...");
        sentences.Enqueue("... But that’s not all! Before you fully complete the first level, one more obstacle awaits you ...");
        sentences.Enqueue("... Up next, you will face Boss Willyfire, whom you must defeat to advance to the next level ...");
        sentences.Enqueue("... Be careful, as the Boss is stronger than any threats you’ve faced so far! ...");
        sentences.Enqueue("Good Luck!"); // Last sentence

        // Show the first sentence
        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        if (sentences.Count == 0)
        {
            Debug.Log("No more sentences left in the queue.");
            StartFadeOut(); // Start fade out after the last sentence
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log("Displaying sentence: " + sentence);
        StartTypingSentence(sentence);
    }

    private void StartTypingSentence(string sentence)
    {
        // Stop previous coroutine if it's active
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            Debug.Log("Stopped previous typing coroutine.");
        }

        // Reset text before typing new sentence
        textComponent.text = "";

        // Start coroutine to type the sentence
        typingCoroutine = StartCoroutine(TypeSentenceCoroutine(sentence));
    }

    private IEnumerator TypeSentenceCoroutine(string sentence)
    {
        Debug.Log("Typing sentence: " + sentence);

        // Type text letter by letter
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.05f); // Typing speed
        }

        Debug.Log("Finished typing sentence: " + sentence);

        // Show next sentence after finishing typing
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
        float fadeDuration = 2.0f; // Duration of fade out effect
        float startAlpha = imageColor.a;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            imageColor.a = Mathf.Lerp(startAlpha, 0, normalizedTime); // Gradually decrease alpha
            imageObject.color = imageColor;
            yield return null;
        }

        // Set alpha to 0 to fully hide the image
        imageColor.a = 0;
        imageObject.color = imageColor;

        // Disable Image object after fade out effect
        imageObject.gameObject.SetActive(false);
        imageObject.enabled = false;
    }
}
