using System.Collections;
using UnityEngine;

public class Level1Music : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayRandomMusic());
    }

    IEnumerator PlayRandomMusic()
    {
        while (true)
        {
            // Play a random song
            AudioManager.instance.PlayRandom("Pixel 2", "Pixel 1");

            // Get the currently playing song duration
            float songDuration = GetCurrentSongDuration();

            // Wait for the song to finish playing
            yield return new WaitForSeconds(songDuration);

            // Wait for a random interval before playing the next song
            yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 60f));
        }
    }

    float GetCurrentSongDuration()
    {
        // Find the currently playing sound and return its duration
        foreach (var sound in AudioManager.instance.sounds)
        {
            if (sound.source.isPlaying && sound.isMusic)
            {
                return sound.clip.length;
            }
        }
        return 0f; // Default duration if no song is found
    }
}
