using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_GiveEffect", menuName = "Scriptable Objects/EnemyActions/GiveEffect", order = 1)]
    public class EnemyAction_GiveEffect : EnemyAction
    {
        [SerializeField] private CharacterEffectBase _characterEffect;
        [SerializeField] protected int _effectDurationInTurns = 1;

        public override string IntentionNumber()
        {
            return "";
        }

        public override bool CanBeTaken()
        {
            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();
            // Remove enemies that already have the effect
            aliveEnemies.Where(e => e.GetCurrentEffects.Any(effect => effect.GetType() == _characterEffect.GetType())).ToList()
                .ForEach(e => aliveEnemies.Remove(e));
            return aliveEnemies.Count > 0;
        }

        public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            // TODO: better logic for choosing who to give buff
            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();

            // Remove enemies that already have the effect
            aliveEnemies.Where(e => e.GetCurrentEffects.Any(effect => effect.GetType() == _characterEffect.GetType())).ToList()
                .ForEach(e => aliveEnemies.Remove(e));

            var enemyToBuff = ListUtilities.GetRandomElement(aliveEnemies);

            enemyToBuff.AddEffect(_characterEffect, _effectDurationInTurns);

            base.TakeAction(enemy, runner, callback);
        }

        protected override void ApplyOnValidate()
        {
            icon = _characterEffect != null ? _characterEffect.EffectIcon : null;

            if (_characterEffect == null)
            {
                return;
            }

            var dict = new Dictionary<string, string>() {
                { "Duration", _effectDurationInTurns.ToString() },
                { "EffectName", _characterEffect.EffectName.GetLocalizedString() }
            };
            translatedDescription.Arguments = new object[] { dict };
        }

    }
}
