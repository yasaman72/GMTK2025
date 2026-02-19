using System;
using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("AddItemToPlayersHand")]
    [System.Serializable]
    public class EnemyAction_AddToHand : EnemyAction
    {
        [SerializeField] private CardEntry cardToAdd;

        public override string IntentionNumber()
        {
            return "";
        }

        public override void TakeAction(IDamageDealer enemy, Action callback = null)
        {
            CardManager.AddCardToHandAction?.Invoke(cardToAdd);

            base.TakeAction(enemy, callback);
        }
    }
}
