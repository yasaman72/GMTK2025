using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("AddHP")]
    [System.Serializable]
    public class AddHP : BaseRelicEffect
    {
        [SerializeField] private int hpToAdd = 10;

        public override bool IsPassive() => true;

        public override void Apply(MonoBehaviour caller)
        {
        }

        public override void OnAdded()
        {
            Player.PlayerCombatCharacter.ExtraMaxHealth += hpToAdd;
            Player.PlayerCombatCharacter.Heal(hpToAdd);
        }

        public override void OnRemoved()
        {
            Player.PlayerCombatCharacter.ExtraMaxHealth -= hpToAdd;
            if (Player.PlayerCombatCharacter.GetCurrentHealth > Player.PlayerCombatCharacter.MaxHealth)
            {
                Player.PlayerCombatCharacter.SetCurrentHealth(Player.PlayerCombatCharacter.MaxHealth);
            }
        }
    }
}
