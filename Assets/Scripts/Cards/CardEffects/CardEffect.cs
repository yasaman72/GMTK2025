
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public abstract class CardEffect
    {
        public abstract void Apply(CombatCharacter target, CardPrefab cardPrefab);
    }

    [System.Serializable]
    public abstract class DamageEnemyCardEffect : CardEffect
    {
        public int damage = 0;
    }

    [AddTypeMenu("DamageEnemy")]
    [System.Serializable]
    public class DamageEnemy : DamageEnemyCardEffect
    {
        public AttackType AttackType = AttackType.Normal;

        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            Player.PlayerCombatCharacter.DealDamage(target, damage, AttackType);
        }
    }

    [AddTypeMenu("DamageAllEnemies")]
    [System.Serializable]
    public class DamageAlllEnemies : DamageEnemyCardEffect
    {
        public AttackType AttackType = AttackType.Normal;

        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            List<Enemy> enemies = CombatManager.SpawnedEnemies.ToList();
            foreach (var enemy in enemies)
            {
                Player.PlayerCombatCharacter.DealDamage(enemy, damage, AttackType);
            }
        }
    }

    [AddTypeMenu("DamagePlayer")]
    [System.Serializable]
    public class DamagePlayer : CardEffect
    {
        public int damage = 0;
        public AttackType AttackType = AttackType.Normal;

        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            IDamageable player = Player.PlayerCombatCharacter;
            player.TakeDamage(damage, AttackType);
        }
    }

    [AddTypeMenu("ApplyEffect")]
    [System.Serializable]
    public class AddCharacterEffect : CardEffect
    {
        public CharacterEffectBase effect;
        public int duration;

        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            target.AddEffect(effect, duration);
        }
    }

    [AddTypeMenu("HealPlayer")]
    [System.Serializable]
    public class HealPlayerEffect : CardEffect
    {
        public int healAmount = 5;

        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            IDamageable player = Player.PlayerCombatCharacter;
            player.Heal(healAmount);
        }
    }

    [AddTypeMenu("ShieldPlayer")]
    [System.Serializable]
    public class ShieldPlayerEffect : CardEffect
    {
        public int shieldAmount = 3;

        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            IDamageable player = Player.PlayerCombatCharacter;
            player.AddShield(shieldAmount);
        }
    }

    [AddTypeMenu("RemoveComponent")]
    [System.Serializable]
    public class RemoveComponentEffect : CardEffect
    {
        public override void Apply(CombatCharacter target, CardPrefab cardPrefab)
        {
            foreach (Transform child in cardPrefab.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
