using System.Collections.Generic;
using UnityEngine;

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
            var guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Data/Materials" });

            var database = AssetDatabase.LoadAssetAtPath<GameDatabase>(
                "Assets/Resources/Database/Database.asset"
            );

            if (database == null)
            {
                Debug.LogError("GameDataBase not found at Assets/Resources/Database/Database.asset");
                return;
            }

            var list = new List<Material>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (asset != null)
                    list.Add(asset);
            }

            database.materials = list.ToArray();

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            Debug.Log($"GameDatabase updated: {list.Count} materials.");
        }
#endif
    }

}
