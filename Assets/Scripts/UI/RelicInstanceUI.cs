using UnityEngine;
using UnityEngine.EventSystems;

namespace Deviloop
{
    public class RelicInstanceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Relic _relic;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.ShowTooltipUnderMouse(_relic.description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}
