using System.IO;
using UnityEngine;

public static class SpritePlatformPrefabCatalog
{
    public const string SpritePlatformsRootPath = "Assets/Sprites/Platforms";
    public const string SpritePlatformPrefabsRootPath = "Assets/Prefabs/Platforms/SpritePlatforms";

    public static string GetPrefabAssetPathFromSpritePath(string spriteAssetPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(spriteAssetPath);
        return $"{SpritePlatformPrefabsRootPath}/{fileName}.prefab";
    }

    public static GameObject LoadPrefabFromSpritePath(string spriteAssetPath)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GetPrefabAssetPathFromSpritePath(spriteAssetPath));
#else
        return null;
#endif
    }
}
