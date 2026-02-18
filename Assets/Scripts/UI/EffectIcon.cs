using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class EffectIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TooltipTrigger _toolTipTrigger;
        [SerializeField] private TextMeshProUGUI _durationText;

        public CharacterEffectBase Effect { get; internal set; }

        internal void Initialize(CharacterEffectBase effect, int duration)
        {
            Effect = effect;
            _image.sprite = effect.EffectIcon;
            _image.color = effect.EffectColor;
            _toolTipTrigger.SetLocalizedString(effect.Description);
            UpdateDurationText(duration);
        }

        public void UpdateDurationText(int duration)
        {
            _durationText.text = duration.ToString();
        }
    }
}
