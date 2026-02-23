using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "MaterialLoot", menuName = "Scriptable Objects/Loots/LootType/Material")]
    public class MaterialLoot : NonCoinLootItem
    {
        public Material materialType;

        public override bool IsSameLoot(NonCoinLootItem other)
        {
            if (other is MaterialLoot otherItemLoot)
            {
                return this.materialType == otherItemLoot.materialType;
            }
            return false;
        }

        public override void ResetLoot()
        {

        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (materialType == null) return;

            icon = materialType.icon;
            itemName = materialType.materialName.GetLocalizedString();
        }
#endif
    }
}
