using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource soundSFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public AudioSource PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool loop = false)
    {
        AudioSource audioSource = Instantiate(soundSFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;

        audioSource.Play();

        if (!loop)
        {
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }

        return audioSource;
    }

    public void StopSFXClip(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            Destroy(audioSource.gameObject); // Clean up the AudioSource object
        }
    }

    public void PlayRandomSFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);
        PlaySFXClip(audioClip[rand], spawnTransform, volume);
    }
}