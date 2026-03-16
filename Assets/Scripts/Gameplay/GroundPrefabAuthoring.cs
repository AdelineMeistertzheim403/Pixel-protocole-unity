using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[DisallowMultipleComponent]
public class GroundPrefabAuthoring : MonoBehaviour
{
    private const string VisualRootName = "Tiles";
    private const string GroundAssetPath = "Assets/Sprites/ground.png";

    [SerializeField] private float width = 24f;
    [SerializeField] private float height = 1.5f;
    [SerializeField] private int sortingOrder;
    [SerializeField] private Color fallbackColor = new Color(0.15f, 0.54f, 0.45f, 1f);
    [SerializeField] private bool autoRebuildInEditor = true;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!autoRebuildInEditor || Application.isPlaying)
        {
            return;
        }

        EditorApplication.delayCall -= DelayedRebuild;
        EditorApplication.delayCall += DelayedRebuild;
#endif
    }

#if UNITY_EDITOR
    private void DelayedRebuild()
    {
        if (this == null || gameObject == null || Application.isPlaying)
        {
            return;
        }

        Rebuild();
    }
#endif

    [ContextMenu("Rebuild Ground")]
    public void Rebuild()
    {
        width = Mathf.Max(0.1f, width);
        height = Mathf.Max(0.1f, height);

        ClearChildren();

        float colliderWidth = width;
        Sprite groundSprite = EditorAssetSpriteLoader.LoadSprite(GroundAssetPath);

        if (groundSprite != null)
        {
            colliderWidth = CreateRepeatedGroundVisuals(groundSprite, width, height);
        }
        else
        {
            GameObject visual = new GameObject("FallbackVisual");
            visual.transform.SetParent(transform, false);
            visual.transform.localScale = new Vector3(width, height, 1f);

            SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateUnitSprite();
            renderer.color = fallbackColor;
            renderer.sortingOrder = sortingOrder;
        }

        BoxCollider2D collider2D = GetOrAddComponent<BoxCollider2D>(gameObject);
        collider2D.size = new Vector2(colliderWidth, height);
        collider2D.offset = Vector2.zero;
    }

    public void Configure(float targetWidth, float targetHeight)
    {
        width = targetWidth;
        height = targetHeight;
        Rebuild();
    }

    private float CreateRepeatedGroundVisuals(Sprite groundSprite, float targetWidth, float targetHeight)
    {
        Vector2 spriteSize = groundSprite.bounds.size;
        if (spriteSize.x <= 0f || spriteSize.y <= 0f)
        {
            return targetWidth;
        }

        GameObject root = new GameObject(VisualRootName);
        root.transform.SetParent(transform, false);

        float uniformScale = targetHeight / spriteSize.y;
        float tileWidth = spriteSize.x * uniformScale;
        int tileCount = Mathf.Max(1, Mathf.CeilToInt(targetWidth / tileWidth));
        float visualWidth = tileCount * tileWidth;
        float leftEdge = -visualWidth * 0.5f;

        for (int i = 0; i < tileCount; i++)
        {
            GameObject tile = new GameObject($"GroundTile_{i:00}");
            tile.transform.SetParent(root.transform, false);
            tile.transform.localPosition = new Vector3(leftEdge + (tileWidth * 0.5f) + (i * tileWidth), 0f, 0f);
            tile.transform.localScale = Vector3.one * uniformScale;

            SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
            renderer.sprite = groundSprite;
            renderer.sortingOrder = sortingOrder;
        }

        return visualWidth;
    }

    private void ClearChildren()
    {
        List<GameObject> children = new List<GameObject>();
        for (int index = 0; index < transform.childCount; index++)
        {
            children.Add(transform.GetChild(index).gameObject);
        }

        foreach (GameObject child in children)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(child);
                continue;
            }
#endif
            Destroy(child);
        }
    }

    private static T GetOrAddComponent<T>(GameObject target) where T : Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.AddComponent<T>();
        }

        return component;
    }

    private static Sprite CreateUnitSprite()
    {
        Texture2D texture = Texture2D.whiteTexture;
        return Sprite.Create(
            texture,
            new UnityEngine.Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            texture.width);
    }
}
