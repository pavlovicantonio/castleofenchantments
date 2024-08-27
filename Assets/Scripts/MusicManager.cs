using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    // TODO - fix song not ending from tutorial level to main menu
    public static MusicManager instance;

    [Tooltip("Assign the music tracks for each level in this array.")]
    public AudioClip[] levelMusicTracks;

    private AudioSource audioSource;
    private int currentLevelIndex = -1;

    public float fadeDuration = 1f;


    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Play music for the initial level
        PlayMusicForLevel(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play music based on the scene index
        PlayMusicForLevel(scene.buildIndex);
    }

    public void PlayMusicForLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelMusicTracks.Length)
        {
            Debug.LogWarning("Invalid level index for music track.");
            return;
        }

        if (levelIndex == currentLevelIndex)
        {
            return;
        }

        StartCoroutine(TransitionMusic(levelIndex));
        currentLevelIndex = levelIndex;
    }

    private IEnumerator TransitionMusic(int newLevelIndex)
    {
        // Fade out current music
        yield return StartCoroutine(FadeOutMusic());

        // Ensure the audio source is stopped before starting new music
        audioSource.Stop();

        // Prepare and fade in new music
        AudioClip newTrack = levelMusicTracks[newLevelIndex];
        if (newTrack != null)
        {
            yield return StartCoroutine(FadeInMusic(newTrack));
        }
        else
        {
            Debug.LogWarning("No music track assigned for this level.");
        }
    }


    public void StopMusic()
    {
        audioSource.Stop();
        currentLevelIndex = -1;
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    private IEnumerator FadeInMusic(AudioClip newTrack)
    {
        audioSource.clip = newTrack;
        audioSource.Play();

        float startVolume = 0f;
        audioSource.volume = startVolume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1;
    }

}
