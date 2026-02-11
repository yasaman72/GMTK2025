using System.Collections.Generic;
using UnityEngine;
using Deviloop;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Deviloop
{

    public class GameDataBaseManager : Singleton<GameDataBaseManager>
    {
        public static GameDatabase GameDatabase;

        private void Start()
        {
            // TODO: could use addressables here instead of Resources.Load
            GameDatabase = Resources.Load<GameDatabase>("Database/Database");
            DontDestroyOnLoad(this.gameObject);
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

            var materialsGuids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Data/Materials" });
            var itemsGuids = AssetDatabase.FindAssets("t:BaseCard", new[] { "Assets/Data/Cards" });
            var relicsGuids = AssetDatabase.FindAssets("t:Relic", new[] { "Assets/Data/Relics" });

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

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            Debug.Log($"GameDatabase updated: {database.materials.Count} materials, {database.cards.Count} cards and {database.relics.Count} relics ");
        }
#endif
    }

}
