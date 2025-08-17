using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;

    private void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayAudio);
        }
    }

    public void PlayAudio()
    {
        AudioManager.OnPlaySoundEffct?.Invoke(_audioClip);
    }
}
