using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[DisallowMultipleComponent]
public class PlayerPrefabAuthoring : MonoBehaviour
{
    [SerializeField] private int sortingOrder = 10;
    [SerializeField] private float visualScale = 4f;
    [SerializeField] private float gravityScale = 4f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float maxFallSpeed = 24f;
    [SerializeField] private Vector2 colliderSize = new Vector2(0.22f, 0.28f);
    [SerializeField] private Vector2 colliderOffset = new Vector2(0f, 0.14f);
    [SerializeField] private Vector3 groundCheckLocalPosition = new Vector3(0f, -0.02f, 0f);
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.42f, 0.08f);
    [SerializeField] private bool autoRebuildInEditor = true;

    private const string GroundCheckName = "GroundCheck";

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

    [ContextMenu("Rebuild Player")]
    public void Rebuild()
    {
        transform.localScale = Vector3.one * visualScale;

        SpriteRenderer renderer = GetOrAddComponent<SpriteRenderer>(gameObject);
        renderer.sortingOrder = sortingOrder;

        Rigidbody2D rb = GetOrAddComponent<Rigidbody2D>(gameObject);
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        BoxCollider2D collider2D = GetOrAddComponent<BoxCollider2D>(gameObject);
        collider2D.size = colliderSize;
        collider2D.offset = colliderOffset;

        Transform groundCheck = GetOrCreateChild(GroundCheckName);
        groundCheck.localPosition = groundCheckLocalPosition;
        groundCheck.localRotation = Quaternion.identity;
        groundCheck.localScale = Vector3.one;

        PlayerController controller = GetOrAddComponent<PlayerController>(gameObject);
        controller.moveSpeed = moveSpeed;
        controller.jumpForce = jumpForce;
        controller.maxFallSpeed = maxFallSpeed;
        controller.groundLayer = Physics2D.AllLayers;
        controller.groundCheck = groundCheck;
        controller.groundCheckSize = groundCheckSize;

        GetOrAddComponent<PlayerSpriteAnimator>(gameObject);
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

    private Transform GetOrCreateChild(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            return child;
        }

        GameObject childObject = new GameObject(childName);
        childObject.transform.SetParent(transform, false);
        return childObject.transform;
    }
}
