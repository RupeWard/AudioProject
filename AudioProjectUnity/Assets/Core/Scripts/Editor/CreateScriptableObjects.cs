using UnityEngine;
using UnityEditor;
using System.IO;

namespace RJWS.Core.EditorExtensions
{
    public static class CreateScriptableObjects
    {
        public static void CreateAsset<T>(string assetName = null) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();

            if (assetName == null)
            {
                assetName = "New " + typeof(T).Name + ".asset";
            }

            ProjectWindowUtil.CreateAsset(asset, assetName);
        }

        [MenuItem("Assets/Create/SO Instance")]
        public static void CreateInstance()
        {
            foreach (Object o in Selection.objects)
            {
                if (o is MonoScript)
                {
                    MonoScript script = (MonoScript)o;
                    System.Type type = script.GetClass();
                    if (type.IsSubclassOf(typeof(ScriptableObject)))
                    {
                        CreateAsset(type, false);
                    }
                }
            }
        }


        [MenuItem("Assets/Create/SO Instance Here")]
        public static void CreateInstanceHere()
        {
            foreach (Object o in Selection.objects)
            {
                if (o is MonoScript)
                {
                    MonoScript script = (MonoScript)o;
                    System.Type type = script.GetClass();
                    if (type.IsSubclassOf(typeof(ScriptableObject)))
                    {
                        CreateAsset(type, true);
                    }
                }
            }
        }

        private static void CreateAsset(System.Type type, bool here)
        {
            var asset = ScriptableObject.CreateInstance(type);

            string assetPathAndName = string.Empty;

            string typeString = type.ToString();
            if (typeString.Contains("."))
            {
                typeString = typeString.Substring(typeString.LastIndexOf(".") + 1);
            }
            if (here)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (path == "")
                {
                    path = "Assets";
                }
                else
                if (Path.GetExtension(path) != "")
                {
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                }
                assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeString + ".asset");
            }
            else
            {
                assetPathAndName = EditorUtility.SaveFilePanel("Create " + typeString + " (Ignore overwrite warnings, AssetDatabase generates new filename)", Application.dataPath, "New" + typeString + ".asset", "");
                if (assetPathAndName.Length > 0)
                {
                    if (!assetPathAndName.EndsWith(".asset"))
                    {
                        EditorUtility.DisplayDialog("Wrong extension", "You are creating an object without the .asset extension. Unity will not recognise it as a ScriptableObject! Rename it or delete and try again.", "ok");
                    }
                    assetPathAndName = "Assets" + assetPathAndName.Replace(Application.dataPath, "");
                    assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPathAndName);
                }
            }
            if (assetPathAndName.Length > 0)
            {
                AssetDatabase.CreateAsset(asset, assetPathAndName);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;

                Debug.Log("Created ScriptableObject of type " + type + " at " + assetPathAndName);
            }
        }
    }

}
