using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource audioSource;

    [Header("Music")]
    public AudioClip victoryMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponentInChildren<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic == null) return;
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = victoryMusic;
        audioSource.Play();
    }
}
