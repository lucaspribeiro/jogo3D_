using UnityEngine;

public class MenuMusicController : MonoBehaviour
{
    public AudioClip menuMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.Play();
    }
}
