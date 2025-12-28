using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "ExtraDamageIfLoopedOneRelicEffect", menuName = "Scriptable Objects/Relic Effects/Extra Damage If Looped One")]
    public class ExtraDamageIfLoopedOneRelicEffect : BaseRelicEffect
    {
        [SerializeField] private int _extraDamage = 3;

        public override void Apply(MonoBehaviour caller)
        {
            if(caller is PlayerLassoManager)
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
