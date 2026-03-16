#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BackgroundPrefabAuthoringEditor
{
    private static readonly Vector2 DefaultBackgroundSize = new Vector2(18f, 10f);

    [MenuItem("Tools/Pixel Protocole/Create Or Update Background Prefabs")]
    public static void CreateOrUpdateBackgroundPrefabs()
    {
        EnsureFolderExists("Assets/Prefabs");
        EnsureFolderExists(BackgroundPrefabCatalog.BackgroundPrefabsRootPath);

        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { BackgroundPrefabCatalog.BackgroundSpritesRootPath });
        foreach (string guid in spriteGuids)
        {
            string spriteAssetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(spriteAssetPath) || spriteAssetPath.EndsWith(".meta"))
            {
                continue;
            }

            CreateOrUpdateBackgroundPrefab(spriteAssetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Background prefabs created or updated in Assets/Prefabs/Backgrounds.");
    }

    private static void CreateOrUpdateBackgroundPrefab(string spriteAssetPath)
    {
        string prefabName = Path.GetFileNameWithoutExtension(spriteAssetPath);
        string prefabAssetPath = BackgroundPrefabCatalog.GetPrefabAssetPathFromSpritePath(spriteAssetPath);

        GameObject root = new GameObject(prefabName);
        BackgroundPrefabAuthoring authoring = root.AddComponent<BackgroundPrefabAuthoring>();
        authoring.Configure(spriteAssetPath, DefaultBackgroundSize, -10);
        PrefabUtility.SaveAsPrefabAsset(root, prefabAssetPath);
        Object.DestroyImmediate(root);
    }

    private static void EnsureFolderExists(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        int separatorIndex = folderPath.LastIndexOf('/');
        string parentPath = separatorIndex > 0 ? folderPath.Substring(0, separatorIndex) : "Assets";
        string folderName = separatorIndex > 0 ? folderPath.Substring(separatorIndex + 1) : folderPath;

        if (!AssetDatabase.IsValidFolder(parentPath))
        {
            EnsureFolderExists(parentPath);
        }

        AssetDatabase.CreateFolder(parentPath, folderName);
    }
}
#endif
