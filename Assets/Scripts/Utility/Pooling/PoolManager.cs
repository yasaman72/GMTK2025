using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class PoolManager : Singleton<PoolManager>
    {
        private readonly Dictionary<Type, object> _pools = new();

        public ObjectPool<T> CreatePool<T>(T prefab, int size) where T : Component
        {
            if (_pools.ContainsKey(prefab.GetType()))
                return (ObjectPool<T>)_pools[prefab.GetType()];

            // create a new parent for each pool to keep the hierarchy organized
            GameObject poolParent = new($"{prefab.name} Pool");
            poolParent.transform.SetParent(transform);
            var pool = new ObjectPool<T>(prefab, size, poolParent.transform);
            _pools[prefab.GetType()] = pool;
            return pool;
        }

        public ObjectPool<T> GetPool<T>(T prefab) where T : Component
        {
            if (_pools.TryGetValue(prefab.GetType(), out var pool))
                return (ObjectPool<T>)pool;
            Debug.LogError($"No pool found for prefab {prefab.name}");
            return null;
        }
    }
}
