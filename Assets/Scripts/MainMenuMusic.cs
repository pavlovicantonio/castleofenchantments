using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    void Start()
    {
        // Stop all music to ensure no previous music continues playing
        FindObjectOfType<AudioManager>().StopAllMusic();

        // Play the main menu music
        FindObjectOfType<AudioManager>().Play("Pixel 4");
    }

}