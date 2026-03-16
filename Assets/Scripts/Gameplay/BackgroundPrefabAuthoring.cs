using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[DisallowMultipleComponent]
public class BackgroundPrefabAuthoring : MonoBehaviour
{
    [SerializeField] private string spriteAssetPath;
    [SerializeField] private int sortingOrder = -10;
    [SerializeField] private Color tint = Color.white;
    [SerializeField] private Vector2 size = new Vector2(16f, 9f);
    [SerializeField] private bool preserveAspect = true;
    [SerializeField] private bool enableParallax = true;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;
    [SerializeField] private Vector2 parallaxFactor = new Vector2(0.2f, 0.08f);
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

    [ContextMenu("Rebuild Background")]
    public void Rebuild()
    {
        size.x = Mathf.Max(0.01f, size.x);
        size.y = Mathf.Max(0.01f, size.y);

        SpriteRenderer renderer = GetOrAddComponent<SpriteRenderer>(gameObject);
        renderer.sortingOrder = sortingOrder;
        renderer.color = tint;
        renderer.sprite = string.IsNullOrEmpty(spriteAssetPath) ? null : EditorAssetSpriteLoader.LoadSprite(spriteAssetPath);

        SyncCameraLock();

        if (renderer.sprite == null)
        {
            transform.localScale = Vector3.one;
            return;
        }

        Vector2 spriteSize = renderer.sprite.bounds.size;
        if (spriteSize.x <= 0f || spriteSize.y <= 0f)
        {
            transform.localScale = Vector3.one;
            return;
        }

        float scaleX = size.x / spriteSize.x;
        float scaleY = size.y / spriteSize.y;

        if (preserveAspect)
        {
            float uniformScale = Mathf.Min(scaleX, scaleY);
            transform.localScale = Vector3.one * uniformScale;
        }
        else
        {
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }

    public void Configure(string spritePath, Vector2 targetSize, int targetSortingOrder)
    {
        spriteAssetPath = spritePath;
        size = targetSize;
        sortingOrder = targetSortingOrder;
        Rebuild();
    }

    private void SyncCameraLock()
    {
        BackgroundCameraLock cameraLock = gameObject.GetComponent<BackgroundCameraLock>();
        if (enableParallax)
        {
            if (cameraLock == null)
            {
                cameraLock = gameObject.AddComponent<BackgroundCameraLock>();
            }

            cameraLock.Configure(Camera.main, followX, followY, parallaxFactor);
            return;
        }

        if (cameraLock != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(cameraLock);
                return;
            }
#endif
            Destroy(cameraLock);
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
}
