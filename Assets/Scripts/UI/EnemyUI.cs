using Deviloop;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : CombatCharacterUI
{
    [Header("Intention")]
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
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        (combatCharacter as Enemy).OnIntentionChanged -= UpdateIntentionUI;
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
            _intentionIcon.enabled = true;
            _intentionIcon.sprite = nextAction.icon;
            _intentionText.text = nextAction.power.ToString();
        }
        else
        {
            _intentionIcon.enabled = false;
            _intentionText.text = "";
        }

        _toolTipTrigger.SetLocalizedString(nextAction.translatedDescription);
    }
}
