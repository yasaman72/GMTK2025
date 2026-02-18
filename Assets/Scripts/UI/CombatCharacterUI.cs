using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public abstract class CombatCharacterUI : CustomMonoBehavior
    {
        [SerializeField] protected CombatCharacter combatCharacter;
        [Header("HP")]
        [SerializeField] protected Slider _hpBar;
        [SerializeField] private Color _onDamageColor = Color.white;
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _hpAmountText;
        [Header("Stats")]
        // TODO: a more generic solution for showing stats on characters
        [SerializeField] private GameObject _shieldIcon;
        [SerializeField] private TextMeshProUGUI _shieldAmountText;

        private Color _originalHpBarColor;

        protected virtual void OnEnable()
        {
            combatCharacter.OnHPChanged += UpdateHpUi;
            combatCharacter.OnShieldChanged += UpdateHpUi;

            if (_fill != null)
                _originalHpBarColor = _fill.color;
        }

        protected virtual void OnDisable()
        {
            combatCharacter.OnHPChanged -= UpdateHpUi;
            combatCharacter.OnShieldChanged -= UpdateHpUi;
        }

        public void UpdateHpUi()
        {
            _hpBar.gameObject.SetActive(combatCharacter.GetCurrentHealth > 0);
            _hpAmountText.gameObject.SetActive(combatCharacter.GetCurrentHealth > 0);

            _hpAmountText.text = $"{combatCharacter.GetCurrentHealth}/{combatCharacter.MaxHealth}";

            float newHPValue = (float)combatCharacter.GetCurrentHealth / combatCharacter.MaxHealth;
            _hpBar.DOValue(newHPValue, 0.25f).SetEase(Ease.OutCubic)
                .OnPlay(() => { _fill.color = _onDamageColor; })
                .OnKill(() => { _fill.color = _originalHpBarColor; });

            _shieldAmountText.text = combatCharacter.GetCurrentShield.ToString();
            _shieldIcon.SetActive(combatCharacter.GetCurrentShield > 0);
        }
    }
}