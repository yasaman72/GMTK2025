using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "LootSet", menuName = "ScriptableObjects/Loots/LootSet", order = 1)]
public class LootSet : ScriptableObject
{
    public List<LootSetData> rewards;


    [System.Serializable]
    public class LootSetData
    {
        public LootItem Item;
        [SerializeField] private int CountMin, CountMax;
        public float Chance;

        private int count = -1;
        public int Count
        {
            get
            {
                if (CountMin == CountMax)
                    count = CountMin;
                if (count == -1)
                    count = Random.Range(CountMin, CountMax + 1);
                return count;
            }
        }

        public void Reset()
        {
            count = -1;
        }

        public void Loot()
        {
            PlayerInventory.AddToInventoryAction?.Invoke(this);
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