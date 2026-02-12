using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Stack<T> _inactiveObjects;
        private readonly List<T> _allObjects;

        public int CountInactive => _inactiveObjects.Count;
        public int CountAll => _allObjects.Count;

        public ObjectPool(T prefab, int initialSize = 0, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _inactiveObjects = new Stack<T>(initialSize);
            _allObjects = new List<T>(initialSize);

            Prewarm(initialSize);
        }

        private void Prewarm(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var instance = CreateInstance();
                ReturnToPool(instance);
            }
        }

        private T CreateInstance()
        {
            var instance = Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            _allObjects.Add(instance);
            return instance;
        }

        public T Get()
        {
            T instance;

            if (_inactiveObjects.Count > 0)
            {
                instance = _inactiveObjects.Pop();
            }
            else
            {
                instance = CreateInstance();
            }

            instance.gameObject.SetActive(true);

            if (instance.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnSpawned();
            }

            return instance;
        }

        public void ReturnToPool(T instance)
        {
            if (!_allObjects.Contains(instance))
            {
                Debug.LogWarning("Trying to return object that does not belong to this pool.");
                return;
            }

            if (_inactiveObjects.Contains(instance))
            {
                Debug.LogWarning("Trying to return object that is already in the pool.");
                return;
            }

            if (instance.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnDespawned();
            }

            instance.gameObject.SetActive(false);
            _inactiveObjects.Push(instance);
        }

        public void Clear()
        {
            foreach (var obj in _allObjects)
            {
                if (obj != null)
                {
                    Object.Destroy(obj.gameObject);
                }
            }

            _inactiveObjects.Clear();
            _allObjects.Clear();
        }
    }
}
