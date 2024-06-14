using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Dodajte ovo


public class TextDisplay : MonoBehaviour
{
    public TMP_Text textComponent; // Referenca na TextMeshPro komponentu
    private Queue<TextSentence> sentences; // Redoslijed rečenica
    private Coroutine typingCoroutine;
    private TextSentence currentSentence;

    // Klasa koja predstavlja rečenicu s pripadajućim uvjetom
    public class TextSentence
    {
        public string text;
        public System.Func<bool> condition;

        public TextSentence(string text, System.Func<bool> condition)
        {
            this.text = text;
            this.condition = condition;
        }
    }

    void Start()
    {
        // Inicijalizacija redoslijeda rečenica
        sentences = new Queue<TextSentence>();

        // Automatski pronađite TextMeshPro komponentu ako nije ručno postavljena
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
        AddTextSentences(new List<TextSentence>
        {
            new TextSentence("Press A to move left or D to move right!", () => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)),
            new TextSentence("Great! Now press W to jump!", () => Input.GetKeyDown(KeyCode.W)),
            new TextSentence("Well done! Press S to crouch.", () => Input.GetKeyDown(KeyCode.S)),
            new TextSentence("Press left click to swing sword!", () => Input.GetMouseButtonDown(0)),
            new TextSentence("Tutorial Complete!", () => false) // Uvjet je false jer je ovo zadnja rečenica
        });

        // Prikaz prve rečenice
        ShowNextSentence();
    }

    void Update()
    {
        if (currentSentence != null && currentSentence.condition())
        {
            ShowNextSentence();
        }
    }

    public void AddTextSentences(List<TextSentence> newSentences)
    {
        foreach (TextSentence sentence in newSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void ShowNextSentence()
    {
        if (textComponent == null)
        {
            Debug.LogError("TextMeshPro component is not assigned.");
            return;
        }

        if (sentences.Count == 0)
        {
            textComponent.text = "Tutorial Complete!";
            StartCoroutine(LoadNextSceneAfterDelay(2.0f)); // Prebacivanje na drugu scenu nakon 2 sekunde
            return;
        }

        currentSentence = sentences.Dequeue();
        StartTypingSentence(currentSentence.text);
    }

    private void StartTypingSentence(string sentence)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentenceCoroutine(sentence));
    }

    private IEnumerator TypeSentenceCoroutine(string sentence)
    {
        textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.05f); // Postavlja brzinu ispisivanja slova
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level1"); // Zamijenite "NextSceneName" s imenom vaše sljedeće scene
    }
}

