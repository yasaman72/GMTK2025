using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("GiveEffect")]
    [System.Serializable]
    public class EnemyAction_GiveEffect : EnemyAction
    {
        [SerializeField] private CharacterEffectBase _characterEffect;
        [SerializeField] protected int _effectDurationInTurns = 1;

        public override string IntentionNumber()
        {
            return "";
        }

        public override bool CanBeTaken(EnemyAction previousAction)
        {
            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();
            // Remove enemies that already have the effect
            aliveEnemies
                .Where(e => e.GetCurrentEffects.Any(effect => effect.GetType() == _characterEffect.GetType()))
                .ToList()
                .ForEach(e => aliveEnemies.Remove(e));
            return aliveEnemies.Count > 0;
        }

        public override void TakeAction(IDamageDealer enemy, Action callback = null)
        {
            // TODO: better logic for choosing who to give buff
            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();

            // Remove enemies that already have the effect
            aliveEnemies.Where(e => e.GetCurrentEffects.Any(effect => effect.GetType() == _characterEffect.GetType())).ToList()
                .ForEach(e => aliveEnemies.Remove(e));

            // TODO: an effect that shows that it couldn't take the action
            if (aliveEnemies.Count <= 0)
            {
                callback?.Invoke();
                return;
            }

            var enemyToBuff = ListUtilities.GetRandomElement(aliveEnemies);

            enemyToBuff.AddEffect(_characterEffect, _effectDurationInTurns);

            base.TakeAction(enemy, callback);
        }

        public override void OnActionSelected()
        {
            var dict = new Dictionary<string, string>() {
                { "Duration", _effectDurationInTurns.ToString() },
                { "EffectName", _characterEffect.EffectName.GetLocalizedString() }
            };
            translatedDescription.Arguments = new object[] { dict };
        }
    }
}
