using UnityEngine;

public static class AudioManagerExtensions
{
    public static void PlayRandom(this AudioManager audioManager, params string[] names)
    {
        if (names.Length == 0) return;

        // Play a random sound
        int index = UnityEngine.Random.Range(0, names.Length);
        audioManager.Play(names[index]);
    }
}
