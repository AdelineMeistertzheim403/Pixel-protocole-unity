using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
public class SpritePlatformPrefabAuthoring : MonoBehaviour
{
    [SerializeField] private string spriteAssetPath;
    [SerializeField] private int sortingOrder = 5;
    [SerializeField] private Color tint = Color.white;
    [SerializeField] private bool autoFitCollider = true;
    [SerializeField] private Vector2 manualColliderSize = new Vector2(1f, 0.25f);
    [SerializeField] private Vector2 manualColliderOffset = Vector2.zero;
    [SerializeField] private bool autoRebuildInEditor = true;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!autoRebuildInEditor || Application.isPlaying)
        {
            return;
        }

        UnityEditor.EditorApplication.delayCall -= DelayedRebuild;
        UnityEditor.EditorApplication.delayCall += DelayedRebuild;
    }

    private void DelayedRebuild()
    {
        if (this == null || gameObject == null || Application.isPlaying)
        {
            return;
        }

        Rebuild();
    }
#endif

    [ContextMenu("Rebuild Sprite Platform")]
    public void Rebuild()
    {
        SpriteRenderer renderer = GetOrAddComponent<SpriteRenderer>(gameObject);
        renderer.sortingOrder = sortingOrder;
        renderer.color = tint;
        renderer.sprite = string.IsNullOrEmpty(spriteAssetPath) ? null : EditorAssetSpriteLoader.LoadSprite(spriteAssetPath);

        BoxCollider2D collider2D = GetOrAddComponent<BoxCollider2D>(gameObject);

        if (renderer.sprite != null && autoFitCollider)
        {
            collider2D.size = renderer.sprite.bounds.size;
            collider2D.offset = renderer.sprite.bounds.center;
        }
        else
        {
            collider2D.size = manualColliderSize;
            collider2D.offset = manualColliderOffset;
        }
    }

    public void Configure(string spritePath, int targetSortingOrder)
    {
        spriteAssetPath = spritePath;
        sortingOrder = targetSortingOrder;
        Rebuild();
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
