using UnityEngine;

public static class PlatformPrefabCatalog
{
    private const string PrefabsRootPath = "Assets/Prefabs/Platforms";

    public static string GetPrefabAssetPath(Tetromino tetromino, int rotationTurns)
    {
        return $"{PrefabsRootPath}/Platform_{tetromino}_R{NormalizeRotation(rotationTurns)}.prefab";
    }

    public static GameObject LoadPrefab(Tetromino tetromino, int rotationTurns)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GetPrefabAssetPath(tetromino, rotationTurns));
#else
        return null;
#endif
    }

    private static int NormalizeRotation(int rotationTurns)
    {
        int normalized = rotationTurns % 4;
        if (normalized < 0)
        {
            normalized += 4;
        }

        return normalized;
    }
}
