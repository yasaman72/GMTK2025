using Cards;
using Cards.ScriptableObjects;
using System;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_Add[Item]ToHand_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/AddToHand", order = 1)]
    public class EnemyAction_AddToHand : EnemyAction
    {
        [SerializeField] private CardEntry cardToAdd;

        public override int IntentionNumber()
        {
            return cardToAdd.Quantity;
        }

        public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            CardManager.AddCardToHandAction?.Invoke(cardToAdd);

            base.TakeAction(enemy, runner, callback);
        }
    }
}
