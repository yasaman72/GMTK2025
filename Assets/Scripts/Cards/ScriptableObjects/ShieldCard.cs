using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShieldCard", menuName = "Cards/Shield Card")]
    public class ShieldCard : BaseCard
    {
        [Header("Shield Properties")]
        public int shieldAmount = 1;
    
        public override void UseCard()
        {
            Debug.Log($"Shield provides {shieldAmount} protection");
            // TODO: Add shield to player
        }
    }
}