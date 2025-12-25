using System.Collections.Generic;
using UnityEngine;
using Cards;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Deviloop
{

    public class GameDataBaseManager : MonoBehaviour
    {
        public static GameDataBaseManager Instance;
        public static GameDatabase GameDatabase;

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            GameDatabase = AssetDatabase.LoadAssetAtPath<GameDatabase>("Assets/Resources/Database/Database.asset");
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public class DataBaseMenuItems
    {
#if UNITY_EDITOR
        [MenuItem("Deviloop/Update All Data")]
        public static void UpdateAllData()
        {
            var database = AssetDatabase.LoadAssetAtPath<GameDatabase>("Assets/Resources/Database/Database.asset");

            if (database == null)
            {
                Debug.LogError("GameDataBase not found at Assets/Resources/Database/Database.asset");
                return;
            }

            var materialsGuids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Data/Materials" });
            var itemsGuids = AssetDatabase.FindAssets("t:BaseCard", new[] { "Assets/Data/Items" });

            var materialsList = new List<Material>();
            var itemsList = new List<BaseCard>();

            foreach (var guid in materialsGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (asset != null)
                    materialsList.Add(asset);
            }

            database.materials = materialsList.ToArray();


            foreach (var guid in itemsGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<BaseCard>(path);
                if (asset != null && asset.isInGame)
                    itemsList.Add(asset);
            }
            database.Cards = itemsList.ToArray();

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            Debug.Log($"GameDatabase updated: {materialsList.Count} materials.");
        }
#endif
    }

}
