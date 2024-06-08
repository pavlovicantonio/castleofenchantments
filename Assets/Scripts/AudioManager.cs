using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private Sound currentlyPlayingMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("AudioManager instance created.");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate AudioManager instance destroyed.");
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }



    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found! Did you spell it correctly?");
            return;
        }

        if (s.isMusic)
        {
            if (currentlyPlayingMusic != null)
            {
                StartCoroutine(FadeOutAndPlayNewMusic(currentlyPlayingMusic, s, 1f));
            }
            else
            {
                currentlyPlayingMusic = s;
                StartCoroutine(FadeIn(currentlyPlayingMusic, 1f));
            }
        }
        else
        {
            s.source.Play();
        }
    }

    IEnumerator FadeOutAndPlayNewMusic(Sound oldSound, Sound newSound, float duration)
    {
        yield return StartCoroutine(FadeOut(oldSound, duration));
        currentlyPlayingMusic = newSound;
        StartCoroutine(FadeIn(newSound, duration));
    }

    IEnumerator FadeIn(Sound sound, float duration)
    {
        if (sound == null || sound.source == null)
        {
            Debug.LogError("FadeIn: Sound or AudioSource is null");
            yield break;
        }

        sound.source.volume = 0f;
        sound.source.Play();
        float targetVolume = sound.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            sound.source.volume = Mathf.Lerp(0f, targetVolume, (Time.time - startTime) / duration);
            yield return null;
        }
        sound.source.volume = targetVolume;
    }

    IEnumerator FadeOut(Sound sound, float duration)
    {
        if (sound == null || sound.source == null)
        {
            Debug.LogError("FadeOut: Sound or AudioSource is null");
            yield break;
        }

        float startVolume = sound.source.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            sound.source.volume = Mathf.Lerp(startVolume, 0f, (Time.time - startTime) / duration);
            yield return null;
        }
        sound.source.Stop();
    }

public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found! Did you spell it correctly?");
            return;
        }
        s.source.Stop();
    }

    public void StopCurrentlyPlayingMusic()
    {
        if (currentlyPlayingMusic != null)
        {
            currentlyPlayingMusic.source.Stop();
            currentlyPlayingMusic = null;
        }
    }

    void Start()
    {
        // Add a debug log to verify the initialization state
        Debug.Log("AudioManager Start method called. Instance ID: " + gameObject.GetInstanceID());
        foreach (Sound s in sounds)
        {
            if (s == null)
            {
                Debug.LogError("Start: Sound element is null in the sounds array.");
            }
            else if (s.source == null)
            {
                Debug.LogError("Start: AudioSource is null for sound: " + s.name);
            }
        }
    }

    public void StopAllMusic()
    {
        foreach (var sound in sounds)
        {
            if (sound.isMusic && sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }

}
