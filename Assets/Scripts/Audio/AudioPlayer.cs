using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isButton;

    private void Start()
    {
        if(_isButton)
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(PlayAudio);
            }
        }
    }

    public void PlayAudio()
    {
        AudioManager.OnPlaySoundEffct?.Invoke(_audioClip);
    }
}
