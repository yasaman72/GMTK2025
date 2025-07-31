using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealingPotionCard", menuName = "Cards/Healing Potion Card")]
    public class HealingPotionCard : BaseCard
    {
        [Header("Healing Properties")]
        public int healAmount = 5;
    
        private void Awake()
        {
            isConsumable = true; // Potions are always consumable
        }
    
        public override void UseCard()
        {
            Debug.Log($"Healing Potion heals player for {healAmount} HP");
            // TODO: Heal player
        }
    }
}