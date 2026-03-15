using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPlayableLevelRuntime : MonoBehaviour
{
    private const string RuntimeRootName = "FirstPlayableLevelRuntime";
    private const string GroundName = "Ground";
    private const string PlayerName = "Player";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void BootstrapScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid())
        {
            return;
        }

        if (Object.FindFirstObjectByType<FirstPlayableLevelRuntime>() != null)
        {
            return;
        }

        if (Object.FindFirstObjectByType<PlayerController>() != null || Object.FindFirstObjectByType<GameManager>() != null)
        {
            return;
        }

        GameObject[] roots = activeScene.GetRootGameObjects();
        if (roots.Length > 1)
        {
            return;
        }

        GameObject runtimeRoot = new GameObject(RuntimeRootName);
        runtimeRoot.AddComponent<FirstPlayableLevelRuntime>();
    }

    void Awake()
    {
        Camera camera = EnsureCamera();
        EnsureGround();
        GameObject player = EnsurePlayer();
        AttachCameraFollow(camera, player.transform);
        Physics2D.SyncTransforms();
    }

    private Camera EnsureCamera()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
        }

        camera.orthographic = true;
        camera.orthographicSize = 4.5f;
        camera.backgroundColor = new Color(0.08f, 0.11f, 0.18f, 1f);
        camera.transform.position = new Vector3(0f, 0f, -10f);

        return camera;
    }

    private GameObject EnsureGround()
    {
        GameObject ground = GameObject.Find(GroundName);
        if (ground != null)
        {
            return ground;
        }

        ground = new GameObject(GroundName);
        ground.transform.position = new Vector3(0f, -3.5f, 0f);
        ground.transform.localScale = new Vector3(24f, 1.5f, 1f);

        SpriteRenderer renderer = ground.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateUnitSprite();
        renderer.color = new Color(0.15f, 0.54f, 0.45f, 1f);
        renderer.sortingOrder = 0;

        BoxCollider2D collider2D = ground.AddComponent<BoxCollider2D>();
        collider2D.size = Vector2.one;

        return ground;
    }

    private GameObject EnsurePlayer()
    {
        PlayerController existingPlayer = Object.FindFirstObjectByType<PlayerController>();
        if (existingPlayer != null)
        {
            return existingPlayer.gameObject;
        }

        GameObject player = new GameObject(PlayerName);
        player.transform.position = new Vector3(-6f, -1.8f, 0f);
        player.transform.localScale = Vector3.one * 4f;

        SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = 10;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 4f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        BoxCollider2D collider2D = player.AddComponent<BoxCollider2D>();
        collider2D.size = new Vector2(0.22f, 0.28f);
        collider2D.offset = new Vector2(0f, 0.14f);

        Transform groundCheck = new GameObject("GroundCheck").transform;
        groundCheck.SetParent(player.transform);
        groundCheck.localPosition = new Vector3(0f, -0.02f, 0f);
        groundCheck.localRotation = Quaternion.identity;
        groundCheck.localScale = Vector3.one;

        PlayerController controller = player.AddComponent<PlayerController>();
        controller.moveSpeed = 6f;
        controller.jumpForce = 13f;
        controller.maxFallSpeed = 20f;
        controller.groundLayer = Physics2D.AllLayers;
        controller.groundCheck = groundCheck;
        controller.groundCheckSize = new Vector2(0.42f, 0.08f);

        player.AddComponent<PlayerSpriteAnimator>();

        return player;
    }

    private void AttachCameraFollow(Camera camera, Transform target)
    {
        CameraFollow2D follow = camera.GetComponent<CameraFollow2D>();
        if (follow == null)
        {
            follow = camera.gameObject.AddComponent<CameraFollow2D>();
        }

        follow.offset = new Vector3(0f, 1.1f, -10f);
        follow.smoothTime = 0.1f;
        follow.SetTarget(target);
    }

    private Sprite CreateUnitSprite()
    {
        Texture2D texture = Texture2D.whiteTexture;
        return Sprite.Create(
            texture,
            new UnityEngine.Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            texture.width);
    }
}
