using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Encounter_Shop_A00_00", menuName = "Scriptable Objects/Encounters/Shop Encounter")]
    public class ShopEncounter : BaseEncounter
    {
        [SerializeField] private ShopData _shopData;

        public override void StartEncounter()
        {
            ShopManager.OnShopStartEvent?.Invoke(_shopData);
            ShopManager.OnShopFinishedEvent += FinishEncounter;
        }

        public override void FinishEncounter()
        {
            ShopManager.OnShopFinishedEvent -= FinishEncounter;
            EncounterManager.OnEncounterFinished?.Invoke();
        }
    }
}
