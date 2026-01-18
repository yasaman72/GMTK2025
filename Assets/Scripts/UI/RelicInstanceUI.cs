using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Deviloop
{
    public class RelicInstanceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Animator _animtor;
        [SerializeField, ReadOnly] private Relic _relic;
        public Relic Relic => _relic;


        private void OnDisable()
        {
            _relic.AfterApply -= OnActivate;
        }

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
            _relic.AfterApply += OnActivate;
        }

        public void OnActivate()
        {
            _animtor.SetTrigger("Activate");
        }
    }
}
