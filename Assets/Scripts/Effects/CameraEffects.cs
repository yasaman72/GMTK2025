using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class CameraEffects : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Image _playerDamageEffect;

        private void OnEnable()
        {
            Player.OnPlayerTookDamage += ShakeCamera;
        }

        private void OnDisable()
        {
            Player.OnPlayerTookDamage -= ShakeCamera;
        }

        private void ShakeCamera(int damage)
        {
            _camera.DOShakePosition(0.2f, .5f, 10, 90, true);
            _playerDamageEffect.gameObject.SetActive(true);
            _playerDamageEffect.DOFade(0.2f, 0.1f).OnComplete(() =>
            {
                _playerDamageEffect.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    _playerDamageEffect.gameObject.SetActive(false);
                });
            });
        }
    }
}
