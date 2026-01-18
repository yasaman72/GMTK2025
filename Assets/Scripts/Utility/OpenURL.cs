using UnityEngine;

namespace Deviloop
{
    public class OpenURL : MonoBehaviour
    {
        [SerializeField, TextArea] private string url;   
        public void Open()
        {
            Application.OpenURL(url);
        }
    }
}
