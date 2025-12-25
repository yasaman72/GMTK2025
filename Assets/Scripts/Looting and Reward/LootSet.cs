using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "LootSet", menuName = "Scriptable Objects/Loots/LootSet", order = 1)]
public class LootSet : ScriptableObject
{
    public List<LootSetData> rewards;


    [System.Serializable]
    public class LootSetData
    {
        public LootItem Item;
        [SerializeField] private int CountMin, CountMax;
        public float Chance;
        public int Count { private set; get; }

        public void Setup()
        {
            Count = Random.Range(CountMin, CountMax + 1);
        }

        public void Loot()
        {
            AudioManager.PlayAudioOneShot?.Invoke(Item.OnLootSound);
            PlayerInventory.AddToInventoryAction?.Invoke(this);
        }

        public LootSetData Clone()
        {
            return new LootSetData
            {
                Item = this.Item,
                Chance = this.Chance,
                Count = this.Count,
                CountMin = this.CountMin,
                CountMax = this.CountMax
            };
        }
    }

    public List<LootSetData> GetPickedLoots()
    {
        List<LootSetData> pickedLoots = new List<LootSetData>();
        foreach (var loot in rewards)
        {
            if (Random.Range(0f, 1f) <= loot.Chance)
            {
                pickedLoots.Add(loot);
            }
        }
        return pickedLoots;
    }

}