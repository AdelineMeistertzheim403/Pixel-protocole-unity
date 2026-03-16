using UnityEngine;

public static class GameplayPrefabCatalog
{
    public const string PlayerPrefabAssetPath = "Assets/Prefabs/Player.prefab";
    public const string GroundPrefabAssetPath = "Assets/Prefabs/Ground.prefab";

    public static GameObject LoadPlayerPrefab()
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabAssetPath);
#else
        return null;
#endif
    }

    public static GameObject LoadGroundPrefab()
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GroundPrefabAssetPath);
#else
        return null;
#endif
    }
}
