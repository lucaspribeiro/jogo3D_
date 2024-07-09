using UnityEngine;
using System.Collections;

public class HorrorMusicController : MonoBehaviour
{
    public AudioClip horrorMusic;
    private AudioSource audioSource;
    public float delay = 10f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = horrorMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        StartCoroutine(PlayMusicWithDelay());
    }

    private IEnumerator PlayMusicWithDelay()
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}
