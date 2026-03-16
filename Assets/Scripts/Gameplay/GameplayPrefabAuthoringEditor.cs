#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class GameplayPrefabAuthoringEditor
{
    private const string PrefabsRootPath = "Assets/Prefabs";

    [MenuItem("Tools/Pixel Protocole/Create Or Update Player And Ground Prefabs")]
    public static void CreateOrUpdatePlayerAndGroundPrefabs()
    {
        EnsureFolderExists(PrefabsRootPath);
        CreateOrUpdatePlayerPrefab();
        CreateOrUpdateGroundPrefab();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Player and Ground prefabs created or updated in Assets/Prefabs.");
    }

    private static void CreateOrUpdatePlayerPrefab()
    {
        GameObject root = new GameObject("Player");
        PlayerPrefabAuthoring authoring = root.AddComponent<PlayerPrefabAuthoring>();
        authoring.Rebuild();
        PrefabUtility.SaveAsPrefabAsset(root, GameplayPrefabCatalog.PlayerPrefabAssetPath);
        Object.DestroyImmediate(root);
    }

    private static void CreateOrUpdateGroundPrefab()
    {
        GameObject root = new GameObject("Ground");
        GroundPrefabAuthoring authoring = root.AddComponent<GroundPrefabAuthoring>();
        authoring.Rebuild();
        PrefabUtility.SaveAsPrefabAsset(root, GameplayPrefabCatalog.GroundPrefabAssetPath);
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
