using UnityEngine;

namespace Deviloop
{
    public class DeleteObjectInRelease : MonoBehaviour
    {
        void Awake()
        {
#if RELEASE
            Destroy(gameObject);
#endif
        }
    }
}
