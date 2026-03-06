using FMODUnity;
using UnityEngine;

namespace Deviloop
{
    public abstract class LootItem : ScriptableObject
    {
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