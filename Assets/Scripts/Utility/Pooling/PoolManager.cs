using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class PoolManager : Singleton<PoolManager>
    {
        private readonly Dictionary<Type, object> _pools = new();
        private readonly Dictionary<string, object> _namePools = new();

        public ObjectPool<T> CreatePool<T>(T prefab, int size, bool byName = false) where T : Component
        {
            if (byName)
            {
                if (_namePools.ContainsKey(prefab.name))
                    return (ObjectPool<T>)_namePools[prefab.name];
            }
            else
            {
                if (_pools.ContainsKey(prefab.GetType()))
                    return (ObjectPool<T>)_pools[prefab.GetType()];
            }

            // create a new parent for each pool to keep the hierarchy organized
            GameObject poolParent = new($"{prefab.name} Pool");
            poolParent.transform.SetParent(transform);
            var pool = new ObjectPool<T>(prefab, size, poolParent.transform);

            if (byName)
                _namePools[prefab.name] = pool;
            else
                _pools[prefab.GetType()] = pool;

            return pool;
        }

        public ObjectPool<T> GetPool<T>(T prefab) where T : Component
        {
            if (_namePools.TryGetValue(prefab.name, out var namePool))
                return (ObjectPool<T>)namePool;

            if (_pools.TryGetValue(prefab.GetType(), out var pool))
                return (ObjectPool<T>)pool;

            throw new KeyNotFoundException($"No pool found for prefab {prefab.name}");
        }
    }
}
