using UnityEngine;

namespace Deviloop
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError($"No instance of {typeof(T)} found in the scene.");
                    }
                }
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"Another instance of {typeof(T)} already exists. Destroying this one.");
                Destroy(gameObject);
            }
        }

    }
}
