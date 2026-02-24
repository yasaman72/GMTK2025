using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Deviloop
{

    public class GameDataBaseManager
    {
        // TODO: could use addressables here instead of Resources.Load
        private static GameDatabase _gameDatabase;
        public static GameDatabase GameDatabase
        {
            get
            {
                if (_gameDatabase == null)
                {
                    _gameDatabase = Resources.Load<GameDatabase>("Database/Database");
                }
                return _gameDatabase;
            }
            set => _gameDatabase = value;
        }
    }

    public class DataBaseMenuItems
    {
#if UNITY_EDITOR
        [MenuItem("Deviloop/Update All Data")]
        public static void UpdateAllData()
        {
            // TODO: a more generalized approach to updating the database
            var database = AssetDatabase.LoadAssetAtPath<GameDatabase>("Assets/Resources/Database/Database.asset");

            if (database == null)
            {
                Debug.LogError("GameDataBase not found at Assets/Resources/Database/Database.asset");
                return;
            }

            database.materials = new List<Material>();
            database.cards = new List<BaseCard>();
            database.relics = new List<Relic>();
            database.rarityConfigs = new List<RarityConfig>();

            var materialsGuids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Data/Materials" });
            var itemsGuids = AssetDatabase.FindAssets("t:BaseCard", new[] { "Assets/Data/Cards" });
            var relicsGuids = AssetDatabase.FindAssets("t:Relic", new[] { "Assets/Data/Relics" });
            var rarityGuids = AssetDatabase.FindAssets("t:RarityConfig", new[] { "Assets/Data/config/Rarity" });

            foreach (var guid in materialsGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (asset != null)
                    database.materials.Add(asset);
            }

            foreach (var guid in itemsGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<BaseCard>(path);
                if (asset != null && asset.isInGame)
                    database.cards.Add(asset);
            }

            foreach (var guid in relicsGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Relic>(path);
                if (asset != null && asset.isInGame)
                    database.relics.Add(asset);
            }

            foreach (var guid in rarityGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<RarityConfig>(path);
                if (asset != null)
                    database.rarityConfigs.Add(asset);
            }

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            Debug.Log($"GameDatabase updated: " +
                $"{database.materials.Count} materials, " +
                $"{database.cards.Count} cards and " +
                $"{database.relics.Count} relics " +
                $"{database.rarityConfigs.Count} rarity configs " +
                $"");
        }
#endif
    }

}
