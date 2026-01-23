using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class MesssageDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void ShowText(string message)
        {
            _text.SetText(message);
            _text.ForceMeshUpdate();
        }
    }
}
