using Deviloop;
using FMODUnity;
using System;
using UnityEngine;


[HelpURL("https://fmod.com/docs/2.02/api/core-api.html")]
public class AudioManager : Singleton<AudioManager>
{
    public static Action<EventReference> PlayAudioOneShot;

    protected override void Awake()
    {
        base.Awake();
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
