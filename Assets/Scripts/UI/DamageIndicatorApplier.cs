using UnityEngine;

namespace Deviloop
{
    public class DamageIndicatorApplier : MonoBehaviour
    {
        [Space]
        [SerializeField] private GameObject _damageIndicator;
        [SerializeField] private Transform _origin;
        [SerializeField] private float _destroyAfter = 1f;
        [SerializeField] private Vector2 _spawnHorizontalRange = new Vector2(-.5f, .5f);

        public void ShowDamageIndicator(int damage, DamageType type = DamageType.Normal)
        {
            Vector2 origin = (Vector2)_origin.position + (Vector2.one * SeededRandom.Range(_spawnHorizontalRange.x, _spawnHorizontalRange.y));

            GameObject damageIndicator = Instantiate(_damageIndicator, origin, Quaternion.identity);
            damageIndicator.GetComponent<DamageIndicator>().Setup(damage, _destroyAfter, type);
        }
    }
}
