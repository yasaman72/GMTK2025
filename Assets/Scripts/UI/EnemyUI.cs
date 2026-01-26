using Deviloop;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : CombatCharacterUI
{
    [Header("Intention")]
    [SerializeField] private GameObject _intentionObject;
    [SerializeField] private Image _intentionIcon;
    [SerializeField] private TextMeshProUGUI _intentionText;
    [SerializeField] private TooltipTrigger _toolTipTrigger;

    private GameObject _canvas;
    private void Awake()
    {
        _canvas = transform.GetChild(0).gameObject;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        (combatCharacter as Enemy).OnIntentionChanged += UpdateIntentionUI;
        combatCharacter.OnEffectAdded += UpdateUIText;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        (combatCharacter as Enemy).OnIntentionChanged -= UpdateIntentionUI;
        combatCharacter.OnEffectAdded -= UpdateUIText;
    }

    private IEnumerator Start()
    {
        _canvas.SetActive(false);
        yield return new WaitForSeconds(1f);
        _canvas.SetActive(true);
    }

    public void UpdateIntentionUI(EnemyAction nextAction)
    {
        if (nextAction)
        {
            _intentionObject.SetActive(true);
            _intentionIcon.enabled = true;
            _intentionIcon.sprite = nextAction.icon;
            _intentionText.text = nextAction.IntentionNumber();
            _toolTipTrigger.SetLocalizedString(nextAction.translatedDescription);
        }
        else
        {
            _intentionObject.SetActive(false);
            _intentionIcon.enabled = false;
            _intentionText.text = "";
        }
    }

    public void UpdateUIText()
    {
        EnemyAction nextAction = (combatCharacter as Enemy).NextAction;

        if (string.IsNullOrEmpty(nextAction.IntentionNumber()))
        {
            _intentionText.text = "";
            _toolTipTrigger.SetLocalizedString(nextAction.translatedDescription);
        }
        else
        {
            int intentionNumber = int.Parse(nextAction.IntentionNumber());
            intentionNumber += combatCharacter.CurrentAttackBuff;
            _intentionText.text = intentionNumber.ToString();

            if (combatCharacter.CurrentAttackBuff > 0)
            {
                _intentionText.color = Color.red;
            }
            else
            {
                _intentionText.color = Color.white;
            }
        }
    }
}
