using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static Action<AudioClip> OnPlaySoundEffct;
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _sfxAudioSource;
    private void Awake()
    {
        // Ensure only one instance of MusicManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        OnPlaySoundEffct += PlayEffect;
    }

    private void OnDisable()
    {
        OnPlaySoundEffct -= PlayEffect;
    }

    private void PlayEffect(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Attempted to play a null audio clip.");
            return;
        }
        _sfxAudioSource.clip = clip;
        _sfxAudioSource.Play();
    }
}
