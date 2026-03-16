#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformPrefabAuthoring))]
public class PlatformPrefabAuthoringEditor : Editor
{
    private const string PrefabsRootPath = "Assets/Prefabs";
    private const string PlatformsRootPath = "Assets/Prefabs/Platforms";
    private const float DefaultTileSize = 0.85f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        PlatformPrefabAuthoring authoring = (PlatformPrefabAuthoring)target;
        if (GUILayout.Button("Rebuild Platform"))
        {
            authoring.RebuildPlatform();
            EditorUtility.SetDirty(authoring.gameObject);
        }

        if (GUILayout.Button("Create Or Update All Platform Prefabs"))
        {
            CreateOrUpdatePlatformPrefabs();
        }
    }

    [MenuItem("Tools/Pixel Protocole/Create Or Update Platform Prefabs")]
    public static void CreateOrUpdatePlatformPrefabs()
    {
        EnsureFolderExists(PrefabsRootPath);
        EnsureFolderExists(PlatformsRootPath);

        foreach (Tetromino tetromino in System.Enum.GetValues(typeof(Tetromino)))
        {
            for (int rotationTurns = 0; rotationTurns < 4; rotationTurns++)
            {
                CreateOrUpdatePrefab(tetromino, rotationTurns);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Platform prefabs created or updated in Assets/Prefabs/Platforms.");
    }

    private static void CreateOrUpdatePrefab(Tetromino tetromino, int rotationTurns)
    {
        string prefabName = $"Platform_{tetromino}_R{rotationTurns}";
        string assetPath = $"{PlatformsRootPath}/{prefabName}.prefab";

        GameObject tempRoot = new GameObject(prefabName);
        PlatformPrefabAuthoring authoring = tempRoot.AddComponent<PlatformPrefabAuthoring>();
        authoring.ApplyPreset(tetromino, rotationTurns, PlatformType.stable, DefaultTileSize);
        authoring.RebuildPlatform();

        PrefabUtility.SaveAsPrefabAsset(tempRoot, assetPath);
        Object.DestroyImmediate(tempRoot);
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
