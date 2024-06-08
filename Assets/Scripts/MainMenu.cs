using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        // Stop all music before loading the next scene
        AudioManager.instance.StopAllMusic();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        // Stop all music before quitting the application
        AudioManager.instance.StopAllMusic();

        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }
}
