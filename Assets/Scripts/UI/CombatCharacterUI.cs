using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatCharacterUI : CustomMonoBehavior
{
    [SerializeField] protected CombatCharacter combatCharacter;
    [Header("HP")]
    [SerializeField] private Slider _hpBar;
    [SerializeField] private TextMeshProUGUI _hpAmountText;
    [Header("Stats")]
    // TODO: a more generic solution for showing stats on characters
    [SerializeField] private GameObject _shieldIcon;
    [SerializeField] private TextMeshProUGUI _shieldAmountText;


    protected virtual void OnEnable()
    {
        combatCharacter.OnHPChanged += UpdateHpUi;
        combatCharacter.OnShieldChanged += UpdateHpUi;
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
        _hpBar.value = (float)combatCharacter.GetCurrentHealth / combatCharacter.MaxHealth;

        _shieldAmountText.text = combatCharacter.GetCurrentShield.ToString();
        _shieldIcon.SetActive(combatCharacter.GetCurrentShield > 0);
    }
}
