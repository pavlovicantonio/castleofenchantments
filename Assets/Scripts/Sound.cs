using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
    public bool loop = false;
    public bool isMusic = false;  // Dodano isMusic varijabla

    [HideInInspector]
    public AudioSource source;
}
