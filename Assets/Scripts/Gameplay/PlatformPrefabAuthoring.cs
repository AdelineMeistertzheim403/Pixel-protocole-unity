using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[DisallowMultipleComponent]
public class PlatformPrefabAuthoring : MonoBehaviour
{
    [SerializeField] private Tetromino tetromino = Tetromino.I;
    [SerializeField] [Range(0, 3)] private int rotationTurns;
    [SerializeField] private PlatformType platformType = PlatformType.stable;
    [SerializeField] private float tileSize = 0.85f;
    [SerializeField] private int sortingOrder = 5;
    [SerializeField] private Sprite overrideSprite;
    [SerializeField] private Color fallbackColor = new Color(0.12f, 0.78f, 0.82f, 1f);
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

        RebuildPlatform();
    }
#endif

    [ContextMenu("Rebuild Platform")]
    public void RebuildPlatform()
    {
        rotationTurns = Mathf.Clamp(rotationTurns, 0, 3);
        tileSize = Mathf.Max(0.01f, tileSize);
        PlatformPrefabBuilder.Rebuild(
            gameObject,
            tetromino,
            rotationTurns,
            tileSize,
            sortingOrder,
            overrideSprite,
            fallbackColor
        );
    }

    public void ApplyPreset(Tetromino tetrominoValue, int rotationTurnsValue, PlatformType platformTypeValue, float tileSizeValue)
    {
        tetromino = tetrominoValue;
        rotationTurns = rotationTurnsValue;
        platformType = platformTypeValue;
        tileSize = tileSizeValue;
    }
}

public static class PlatformPrefabBuilder
{
    private const string BlocksRootName = "Blocks";
    private const string VisualName = "Visual";
    private static Sprite fallbackSprite;

    public static void Rebuild(
        GameObject root,
        Tetromino tetromino,
        int rotationTurns,
        float tileSize,
        int sortingOrder,
        Sprite overrideSprite,
        Color fallbackColor
    )
    {
        if (root == null || !Logic.SHAPES.TryGetValue(tetromino, out Vector2Int[] shape))
        {
            return;
        }

        ClearGeneratedChildren(root.transform);

        Vector2Int[] blocks = RotateBlocks(shape, rotationTurns);
        Bounds bounds = CalculateBounds(blocks, tileSize);

        GameObject blocksRoot = new GameObject(BlocksRootName);
        blocksRoot.transform.SetParent(root.transform, false);

        foreach (Vector2Int block in blocks)
        {
            GameObject blockObject = new GameObject($"Block_{block.x}_{block.y}");
            blockObject.transform.SetParent(blocksRoot.transform, false);
            blockObject.transform.localPosition = GetCenteredLocalPosition(block, bounds, tileSize);

            BoxCollider2D collider = blockObject.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one * tileSize;
        }

        GameObject visual = new GameObject(VisualName);
        visual.transform.SetParent(root.transform, false);

        SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = sortingOrder;
        renderer.sprite = overrideSprite != null ? overrideSprite : PlatformVisualLibrary.LoadSprite(tetromino);

        if (renderer.sprite != null)
        {
            Vector2 spriteSize = renderer.sprite.bounds.size;
            float scaleX = spriteSize.x > 0f ? bounds.size.x / spriteSize.x : 1f;
            float scaleY = spriteSize.y > 0f ? bounds.size.y / spriteSize.y : 1f;
            float uniformScale = Mathf.Min(scaleX, scaleY);
            visual.transform.localScale = Vector3.one * uniformScale;
            renderer.color = Color.white;
        }
        else
        {
            renderer.sprite = GetFallbackSprite();
            renderer.color = fallbackColor;
            visual.transform.localScale = new Vector3(bounds.size.x, bounds.size.y, 1f);
        }
    }

    private static void ClearGeneratedChildren(Transform root)
    {
        List<GameObject> generatedObjects = new List<GameObject>();
        for (int index = 0; index < root.childCount; index++)
        {
            generatedObjects.Add(root.GetChild(index).gameObject);
        }

        foreach (GameObject generatedObject in generatedObjects)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(generatedObject);
                continue;
            }
#endif
            Object.Destroy(generatedObject);
        }
    }

    private static Vector2Int[] RotateBlocks(Vector2Int[] baseShape, int rotationTurns)
    {
        Vector2Int[] rotated = new Vector2Int[baseShape.Length];
        for (int index = 0; index < baseShape.Length; index++)
        {
            rotated[index] = Logic.Rotate(baseShape[index], rotationTurns);
        }

        return rotated;
    }

    private static Bounds CalculateBounds(Vector2Int[] blocks, float tileSize)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (Vector2Int block in blocks)
        {
            minX = Mathf.Min(minX, block.x);
            minY = Mathf.Min(minY, block.y);
            maxX = Mathf.Max(maxX, block.x);
            maxY = Mathf.Max(maxY, block.y);
        }

        Vector3 center = new Vector3(
            ((minX + maxX + 1f) * 0.5f - 0.5f) * tileSize,
            ((minY + maxY + 1f) * 0.5f - 0.5f) * tileSize,
            0f
        );

        Vector3 size = new Vector3(
            (maxX - minX + 1f) * tileSize,
            (maxY - minY + 1f) * tileSize,
            tileSize
        );

        return new Bounds(center, size);
    }

    private static Vector3 GetCenteredLocalPosition(Vector2Int block, Bounds bounds, float tileSize)
    {
        return new Vector3(
            (block.x * tileSize) - bounds.center.x,
            (block.y * tileSize) - bounds.center.y,
            0f
        );
    }

    private static Sprite GetFallbackSprite()
    {
        if (fallbackSprite != null)
        {
            return fallbackSprite;
        }

        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        fallbackSprite = Sprite.Create(texture, new UnityEngine.Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        return fallbackSprite;
    }
}
