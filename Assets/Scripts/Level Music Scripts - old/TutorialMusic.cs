using UnityEngine;

public class TutorialMusic : MonoBehaviour
{
    void Start()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play("Pixel 6");
        }
    }
}