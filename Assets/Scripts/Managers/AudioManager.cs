using FMODUnity;
using System;
using UnityEngine;


[HelpURL("https://fmod.com/docs/2.02/api/core-api.html")]
public class AudioManager : MonoBehaviour
{
    public static Action<EventReference> PlayAudioOneShot;
    public static AudioManager Instance { get; private set; }

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
        PlayAudioOneShot += PlayOneShot;
    }

    private void OnDisable()
    {
        PlayAudioOneShot -= PlayOneShot;
    }

    private void PlayOneShot(EventReference clip)
    {
        RuntimeManager.PlayOneShot(clip);
    }
}
