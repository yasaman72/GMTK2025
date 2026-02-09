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
    private IntentionUIData _intentionUIData;

    public struct IntentionUIData
    {
        public bool isAttackBuffed;
    }

    private void Awake()
    {
        _canvas = transform.GetChild(0).gameObject;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        (combatCharacter as Enemy).OnIntentionChanged += UpdateIntentionUI;
        combatCharacter.OnAttackBuffApplied += OnAttackBuffApplied;
    }



    protected override void OnDisable()
    {
        base.OnDisable();
        (combatCharacter as Enemy).OnIntentionChanged -= UpdateIntentionUI;
        combatCharacter.OnAttackBuffApplied -= OnAttackBuffApplied;
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

            if (_intentionUIData.isAttackBuffed && nextAction is EnemyAction_Attack)
            {
                int intentionNumber = int.Parse(nextAction.IntentionNumber());
                intentionNumber += combatCharacter.CurrentAttackBuff;
                _intentionText.text = intentionNumber.ToString();

                _intentionText.color = Color.red;
            }
            else
            {
                _intentionText.color = Color.white;
            }
        }
        else
        {
            _intentionObject.SetActive(false);
            _intentionIcon.enabled = false;
            _intentionText.text = "";
        }
    }

    private void OnAttackBuffApplied(bool isApplied)
    {
        _intentionUIData.isAttackBuffed = isApplied;
        UpdateIntentionUI((combatCharacter as Enemy).CurrentAction);
    }
}
