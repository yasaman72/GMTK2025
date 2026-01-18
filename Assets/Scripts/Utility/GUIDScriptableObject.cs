using UnityEditor;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "GUIDScriptableObject", menuName = "Scriptable Objects/GUIDScriptableObject")]
    public class GUIDScriptableObject : ScriptableObject
    {
        [ReadOnly] public string GUID;

#if UNITY_EDITOR
        [ContextMenu("Update GUID")]
        protected void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            if (!string.IsNullOrEmpty(path))
            {
                GUID = AssetDatabase.AssetPathToGUID(path);
            }
        }
#endif
    }
}
