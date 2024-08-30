using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] private AudioClip mainMenuClip;

    public void Play()
    {
        // Stop all music before loading the next scene
        //MusicManager.instance.PlayMusic(mainMenuClip, 1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        // Stop all music before quitting the application
        MusicManager.instance.StopMusic();

        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }
}
