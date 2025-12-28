using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Deviloop
{
    public class RelicInstanceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField, ReadOnly] private Relic _relic;
        public Relic Relic => _relic;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.ShowTooltipUnderMouse(_relic.description.GetLocalizedString());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.HideTooltip();
        }

        public void Setup(Relic relic)
        {
            _relic = relic;
            _image.sprite = relic.icon;
        }
    }
}
