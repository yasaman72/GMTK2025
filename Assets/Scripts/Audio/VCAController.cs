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
            // TODO: set the volume properly, currently just halves the value
            VcaController.setVolume(newVolume / 2);
        }
    }
}
