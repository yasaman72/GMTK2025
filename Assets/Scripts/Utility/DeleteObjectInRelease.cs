using System;
using UnityEngine;

namespace Deviloop
{
    public class DeleteObjectInRelease : MonoBehaviour
    {
        void Awake()
        {
#if DEBUG
#else
            Destroy(gameObject);
#endif
        }
    }
}
