using System.IO;
using UnityEngine;

public static class BackgroundPrefabCatalog
{
    public const string BackgroundSpritesRootPath = "Assets/Sprites/PixelProtocol/backgrounds";
    public const string BackgroundPrefabsRootPath = "Assets/Prefabs/Backgrounds";

    public static string GetPrefabAssetPathFromSpritePath(string spriteAssetPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(spriteAssetPath);
        return $"{BackgroundPrefabsRootPath}/{fileName}.prefab";
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
