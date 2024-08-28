using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuClip;

    void Start()
    {
        //MusicManager.instance.PlayMusic(mainMenuClip, 1f); // Use MusicManager to play the background music
    }

    void OnDestroy()
    {
        //MusicManager.instance.StopMusic(); // Stop the music when the scene is destroyed
    }
}


// Stop all music to ensure no previous music continues playing
//FindObjectOfType<AudioManager>().StopAllMusic();

// Play the main menu music
//FindObjectOfType<AudioManager>().Play("Pixel 4");