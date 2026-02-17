using System;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "[Card]", menuName = "Cards/Custom Behavior Card")]
    public class CardWithCustomComponent : BaseCard
    {
        [SerializeField] private GameObject _extentionPrefab;

        public void AddComponent(GameObject cardPrefab)
        {
            if (cardPrefab.GetComponentAtIndex(0).name == _extentionPrefab.name)
            {
                return;
            }

            GameObject instance = Instantiate(_extentionPrefab);
            instance.transform.SetParent(cardPrefab.transform, true);
            instance.transform.localPosition = Vector3.zero;
        }

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback?.Invoke();
        }
    }
}
