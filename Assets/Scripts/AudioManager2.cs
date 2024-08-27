using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip wallSlide;
    public AudioClip portalIn;
    public AudioClip portalOut;

}
