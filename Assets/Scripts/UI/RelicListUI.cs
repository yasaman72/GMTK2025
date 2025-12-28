using UnityEngine;

namespace Deviloop
{
    public class RelicListUI : MonoBehaviour
    {
        [SerializeField] private GameObject _relicPrefab;

        private void OnEnable()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var relics = RelicManager.OwnedRelics;
            foreach (var relic in relics)
            {
                var relicUIObj = Instantiate(_relicPrefab, transform);
                var relicUI = relicUIObj.GetComponent<RelicInstanceUI>();
                relicUI.Setup(relic);
            }

            RelicManager.OnRelicAdded += OnRelicAdded;
            RelicManager.OnRelicRemoved += OnRelicRemoved;
        }
        private void OnDisable()
        {
            RelicManager.OnRelicAdded -= OnRelicAdded;
            RelicManager.OnRelicRemoved -= OnRelicRemoved;
        }

        private void OnRelicRemoved(Relic relic)
        {
            foreach (Transform child in transform)
            {
                var relicUI = child.GetComponent<RelicInstanceUI>();
                if (relicUI != null && relicUI.Relic == relic)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }

        private void OnRelicAdded(Relic relic)
        {
            var relicUIObj = Instantiate(_relicPrefab, transform);
            var relicUI = relicUIObj.GetComponent<RelicInstanceUI>();
            relicUI.Setup(relic);
        }
    }
}
