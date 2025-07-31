using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AxeCard", menuName = "Cards/Axe Card")]
    public class AxeCard : BaseCard
    {
        [Header("Axe Properties")]
        public int damage = 3;
    
        public override void UseCard()
        {
            Debug.Log($"Axe deals {damage} damage to selected enemy");
            // TODO: Apply damage to selected target
        }
    }
}