using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BombCard", menuName = "Cards/Bomb Card")]
    public class BombCard : BaseCard
    {
        [Header("Bomb Properties")]
        public int damage = 3;
    
        public override void UseCard()
        {
            Debug.Log($"Bomb explodes! Player takes {damage} damage");
            // TODO: Damage player
        }
    }
}