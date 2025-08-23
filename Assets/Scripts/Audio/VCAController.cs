using UnityEngine;

namespace Deviloop
{
    public class VCAController : MonoBehaviour
    {
        [SerializeField] private string VcaName;
        private FMOD.Studio.VCA VcaController;

        private void Start()
        {
            VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
        }

        public void SetVolume(float newVolume)
        {
            VcaController.setVolume(newVolume);
        }
    }
}
