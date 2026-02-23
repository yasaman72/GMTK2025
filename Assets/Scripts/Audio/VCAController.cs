using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    [HelpURL("https://fmod.com/docs/2.02/api/studio-api-vca.html")]
    public class VCAController : MonoBehaviour, IInitiatable
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private string _vCAaName;

        private string _volumeKey;
        private FMOD.Studio.VCA VcaController;

        public void Initiate()
        {
            _volumeKey = _volumeKey + "Volume";

            VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + _vCAaName);

            if (PlayerPrefs.HasKey(_volumeKey))
            {
                VcaController.setVolume(PlayerPrefs.GetFloat(_volumeKey, 1f));
            }
            else
            {
                VcaController.setVolume(.5f);
                PlayerPrefs.SetFloat(_volumeKey, .5f);
            }
        }
        
        public void Deactivate()
        {
        }

        private void Start()
        {
            _slider.onValueChanged.AddListener(SetVolume);
        }

        private void OnEnable()
        {
            VcaController.getVolume(out var currentVolume);
            _slider.value = currentVolume;
        }

        public void SetVolume(float newVolume)
        {
            // TODO: set the volume properly, currently just halves the value
            VcaController.setVolume(newVolume);
            PlayerPrefs.SetFloat(_volumeKey, newVolume);
        }
    }
}
