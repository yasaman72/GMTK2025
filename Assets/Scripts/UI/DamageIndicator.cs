using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Deviloop
{
    public enum DamageType
    {
        Normal,
        Critical,
        Heal,
        Shield
    }

    public class DamageIndicator : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Animator animator;
        [SerializeField] private DamageTypeSettings[] _damageTypeSettings;

        [System.Serializable]
        public struct DamageTypeSettings
        {
            public Color color;
            public DamageType damageType;
        }

        public void Setup(int amount, float _destroyAfter, DamageType damageType)
        {
            _text.color = System.Array.Find(_damageTypeSettings, setting => setting.damageType == damageType).color;

            _text.text = amount.ToString();
            if (_destroyAfter <= 0)
            {
                _destroyAfter = 1f;
            }
            else
            {
                animator.speed = 1f / _destroyAfter;

            }

            DestroyDamageIndicator(_destroyAfter);
        }

        private async Task DestroyDamageIndicator(float _destroyAfter)
        {
            await Awaitable.WaitForSecondsAsync(_destroyAfter);
            PoolManager.Instance.GetPool(this).ReturnToPool(this);
        }

        public void OnSpawned()
        {
        }

        public void OnDespawned()
        {
        }
    }
}
