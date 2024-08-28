using UnityEngine;

public class Level2Music : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Pixel 11");
    }
}