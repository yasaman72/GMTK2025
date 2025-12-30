using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("ExtraDamageIfLoopedOne")]
    [System.Serializable]
    public class ExtraDamageIfLoopedOne : BaseRelicEffect
    {
        [SerializeField] private int _extraDamage = 3;

        public override void Apply(MonoBehaviour caller)
        {
            if (caller is PlayerLassoManager)
            {
                if (PlayerLassoManager.lassoedCardsCount == 1)
                {
                    var enemies = CombatManager.SpawnedEnemies;

                    foreach (var enemy in enemies)
                    {
                        Player.PlayerCombatCharacter.DealDamage(enemy, _extraDamage);
                    }
                }
            }
        }

        public override void OnAdded()
        {
        }

        public override void OnRemoved()
        {
        }
    }
}
