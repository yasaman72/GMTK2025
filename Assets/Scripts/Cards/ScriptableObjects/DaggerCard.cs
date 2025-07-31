using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DaggerCard", menuName = "Cards/Dagger Card")]
    public class DaggerCard : BaseCard
    {
        [Header("Dagger Properties")]
        public int damage = 1;
    
        public override void UseCard()
        {
            Debug.Log($"Dagger deals {damage} damage to selected enemy");
            // TODO: Apply damage to selected target
        }
    }
}