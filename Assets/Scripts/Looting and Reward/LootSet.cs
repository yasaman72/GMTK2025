using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "LootSet", menuName = "Scriptable Objects/Loots/LootSet", order = 1)]
public class LootSet : ScriptableObject
{
    public int minLootPicked = 2;
    public List<LootSetData> rewards;


    [System.Serializable]
    public class LootSetData
    {
        public LootItem Item;
        [SerializeField] private int CountMin, CountMax;
        public float Chance;

        private int _count = 0;
        public int Count
        {
            set { _count = value; }
            get
            {
                if ((_count == 0))
                {
                    Count = Random.Range(CountMin, CountMax + 1);
                }
                return _count;
            }
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

    public List<LootSetData> GetPickedLoots(int count = 1)
    {
        count = Mathf.Max(count, minLootPicked);

        List<LootSetData> pickedLoots = new List<LootSetData>();

        float totalChance = 0f;
        foreach (var loot in rewards)
            totalChance += loot.Chance;

        for (int i = 0; i < count; i++)
        {
            float roll = Random.Range(0f, totalChance);
            float cumulative = 0f;

            foreach (var loot in rewards)
            {
                cumulative += loot.Chance;
                if (roll <= cumulative)
                {
                    pickedLoots.Add(loot);
                    break;
                }
            }
        }

        return pickedLoots;
    }

}