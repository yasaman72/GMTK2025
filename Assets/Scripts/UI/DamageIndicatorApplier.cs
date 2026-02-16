using System;
using UnityEngine;

namespace Deviloop
{
    public class DamageIndicatorApplier : MonoBehaviour
    {
        [Space]
        [SerializeField] private DamageIndicator _damageIndicator;
        [SerializeField] private Transform _origin;
        [SerializeField] private float _destroyAfter = 1f;
        [SerializeField] private Vector2 _spawnHorizontalRange = new Vector2(-.5f, .5f);

        ObjectPool<DamageIndicator> _pool;

        private void OnEnable()
        {
            try
            {
                _pool = PoolManager.Instance.GetPool(_damageIndicator);
            }
            catch (Exception)
            {
                _pool = PoolManager.Instance.CreatePool(_damageIndicator, 8);
            }
        }

        public void ShowDamageIndicator(int damage, DamageType type = DamageType.Normal)
        {
            Vector2 origin = (Vector2)_origin.position + (Vector2.one * SeededRandom.Range(_spawnHorizontalRange.x, _spawnHorizontalRange.y));

            DamageIndicator damageIndicator = _pool.Get();
            damageIndicator.transform.position = origin;
            damageIndicator.transform.rotation = Quaternion.identity;
            damageIndicator.GetComponent<DamageIndicator>().Setup(damage, _destroyAfter, type);
        }
    }
}
