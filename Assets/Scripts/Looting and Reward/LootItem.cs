using Deviloop;
using FMODUnity;
using UnityEngine;

namespace Deviloop
{
    public abstract class LootItem : ScriptableObject
    {
        public string itemName;
        [TextArea(3, 10)]
        public string description;
        public Sprite icon;
        public EventReference OnLootSound;

        [DeveloperNotes, SerializeField]
        private string developerNotes;
    }


    // TODO: come up with a better name
    public abstract class NonCoinLootItem : LootItem
    {
        public abstract void ResetLoot();
        public abstract  bool IsSameLoot(NonCoinLootItem other);
    }
}