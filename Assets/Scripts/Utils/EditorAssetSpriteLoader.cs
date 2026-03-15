using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class EditorAssetSpriteLoader
{
    public static Texture2D LoadTexture(string assetPath)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
#else
        return null;
#endif
    }

    public static Sprite LoadSprite(string assetPath)
    {
#if UNITY_EDITOR
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if (sprite != null)
        {
            return sprite;
        }

        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        foreach (Object asset in assets)
        {
            if (asset is Sprite loadedSprite)
            {
                return loadedSprite;
            }
        }
#endif

        return null;
    }
}
