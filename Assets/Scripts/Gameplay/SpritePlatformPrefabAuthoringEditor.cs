#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpritePlatformPrefabAuthoringEditor
{
    [MenuItem("Tools/Pixel Protocole/Create Or Update Sprite Platform Prefabs")]
    public static void CreateOrUpdateSpritePlatformPrefabs()
    {
        EnsureFolderExists("Assets/Prefabs");
        EnsureFolderExists("Assets/Prefabs/Platforms");
        EnsureFolderExists(SpritePlatformPrefabCatalog.SpritePlatformPrefabsRootPath);

        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { SpritePlatformPrefabCatalog.SpritePlatformsRootPath });
        foreach (string guid in spriteGuids)
        {
            string spriteAssetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(spriteAssetPath) || spriteAssetPath.EndsWith(".meta"))
            {
                continue;
            }

            CreateOrUpdateSpritePlatformPrefab(spriteAssetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Sprite platform prefabs created or updated in Assets/Prefabs/Platforms/SpritePlatforms.");
    }

    private static void CreateOrUpdateSpritePlatformPrefab(string spriteAssetPath)
    {
        string prefabName = Path.GetFileNameWithoutExtension(spriteAssetPath);
        string prefabAssetPath = SpritePlatformPrefabCatalog.GetPrefabAssetPathFromSpritePath(spriteAssetPath);

        GameObject root = new GameObject(prefabName);
        SpritePlatformPrefabAuthoring authoring = root.AddComponent<SpritePlatformPrefabAuthoring>();
        authoring.Configure(spriteAssetPath, 5);
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
