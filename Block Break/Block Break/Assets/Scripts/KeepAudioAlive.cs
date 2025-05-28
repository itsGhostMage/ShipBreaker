using UnityEngine;

public class KeepAudioAlive : MonoBehaviour
{
    public static KeepAudioAlive Instance { get; private set; }
    private AudioSource _audioSource;

    void Awake()
    {
        // Handle singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get audio source component
        _audioSource = GetComponent<AudioSource>();

        // Start playing if not already playing
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}