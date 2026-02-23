using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public abstract class CardOnLassoEffect
    {
        public abstract void Apply(CardPrefab cardPrefab);
    }

    [AddTypeMenu("RemoveComponent")]
    [System.Serializable]
    public class RemoveComponentEffect : CardOnLassoEffect
    {
        public override void Apply(CardPrefab cardPrefab)
        {
            foreach (Transform child in cardPrefab.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
