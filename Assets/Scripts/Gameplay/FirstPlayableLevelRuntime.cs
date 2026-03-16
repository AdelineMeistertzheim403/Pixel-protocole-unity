using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPlayableLevelRuntime : MonoBehaviour
{
    private const string RuntimeRootName = "FirstPlayableLevelRuntime";
    private const string GroundName = "Ground";
    private const string PlatformsRootName = "Platforms";
    private const string PlayerName = "Player";
    private const string GroundAssetPath = "Assets/Sprites/ground.png";
    private const float DefaultGroundWidth = 24f;
    private const float GroundHeight = 1.5f;
    private const float GroundY = -3.5f;
    private const float PlatformTileSize = 0.85f;

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
        EnsurePlatforms();
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

        GameObject groundPrefab = GameplayPrefabCatalog.LoadGroundPrefab();
        if (groundPrefab != null)
        {
            ground = Instantiate(groundPrefab);
            ground.name = GroundName;
            ground.transform.position = new Vector3(0f, GroundY, 0f);

            GroundPrefabAuthoring authoring = ground.GetComponent<GroundPrefabAuthoring>();
            if (authoring != null)
            {
                authoring.Configure(GetGroundWidth(), GroundHeight);
            }

            return ground;
        }

        ground = new GameObject(GroundName);
        ground.transform.position = new Vector3(0f, GroundY, 0f);

        float groundWidth = GetGroundWidth();
        float colliderWidth = groundWidth;
        Sprite groundSprite = EditorAssetSpriteLoader.LoadSprite(GroundAssetPath);

        if (groundSprite != null)
        {
            colliderWidth = CreateRepeatedGroundVisuals(ground.transform, groundSprite, groundWidth, GroundHeight);
        }
        else
        {
            SpriteRenderer renderer = ground.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateUnitSprite();
            renderer.color = new Color(0.15f, 0.54f, 0.45f, 1f);
            renderer.sortingOrder = 0;
            ground.transform.localScale = new Vector3(groundWidth, GroundHeight, 1f);
        }

        BoxCollider2D collider2D = ground.AddComponent<BoxCollider2D>();
        collider2D.size = new Vector2(colliderWidth, GroundHeight);

        return ground;
    }

    private GameObject EnsurePlatforms()
    {
        GameObject existing = GameObject.Find(PlatformsRootName);
        if (existing != null)
        {
            return existing;
        }

        LevelDef level = GetCurrentLevel();
        if (level == null || level.platforms == null || level.platforms.Count == 0)
        {
            return null;
        }

        GameObject root = new GameObject(PlatformsRootName);
        foreach (PlatformDef platform in level.platforms)
        {
            CreatePlatform(root.transform, platform);
        }

        return root;
    }

    private void CreatePlatform(Transform parent, PlatformDef platform)
    {
        Vector2Int[] blocks = GetPlatformBlocks(platform);
        if (blocks.Length == 0)
        {
            return;
        }

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (Vector2Int block in blocks)
        {
            float blockX = platform.x + block.x;
            float blockY = platform.y + block.y;
            minX = Mathf.Min(minX, blockX);
            maxX = Mathf.Max(maxX, blockX);
            minY = Mathf.Min(minY, blockY);
            maxY = Mathf.Max(maxY, blockY);
        }

        float centerTileX = (minX + maxX + 1f) * 0.5f;
        float centerTileY = (minY + maxY + 1f) * 0.5f;
        float targetWidth = (maxX - minX + 1f) * PlatformTileSize;
        float targetHeight = (maxY - minY + 1f) * PlatformTileSize;

        GameObject platformObject = new GameObject($"Platform_{platform.id}");
        platformObject.transform.SetParent(parent, false);
        platformObject.transform.position = new Vector3(TileToWorldX(centerTileX), TileToWorldY(centerTileY), 0f);

        foreach (Vector2Int block in blocks)
        {
            float blockCenterX = (platform.x + block.x + 0.5f) - centerTileX;
            float blockCenterY = (platform.y + block.y + 0.5f) - centerTileY;

            GameObject colliderObject = new GameObject($"Block_{block.x}_{block.y}");
            colliderObject.transform.SetParent(platformObject.transform, false);
            colliderObject.transform.localPosition = new Vector3(blockCenterX * PlatformTileSize, blockCenterY * PlatformTileSize, 0f);

            BoxCollider2D collider2D = colliderObject.AddComponent<BoxCollider2D>();
            collider2D.size = Vector2.one * PlatformTileSize;
        }

        Sprite platformSprite = LoadPlatformSprite(platform.tetromino);
        if (platformSprite != null)
        {
            CreatePlatformVisual(platformObject.transform, platformSprite, targetWidth, targetHeight, GetRotationTurns(platform));
        }
        else
        {
            CreateFallbackPlatformVisual(platformObject.transform, targetWidth, targetHeight);
        }
    }

    private void CreatePlatformVisual(Transform parent, Sprite platformSprite, float targetWidth, float targetHeight, int turns)
    {
        Vector2 spriteSize = platformSprite.bounds.size;
        if (spriteSize.x <= 0f || spriteSize.y <= 0f)
        {
            CreateFallbackPlatformVisual(parent, targetWidth, targetHeight);
            return;
        }

        Vector2 rotatedSpriteSize = turns % 2 == 0
            ? spriteSize
            : new Vector2(spriteSize.y, spriteSize.x);

        float uniformScale = Mathf.Min(targetWidth / rotatedSpriteSize.x, targetHeight / rotatedSpriteSize.y);

        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(parent, false);
        visual.transform.localRotation = Quaternion.Euler(0f, 0f, 90f * turns);
        visual.transform.localScale = Vector3.one * uniformScale;

        SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
        renderer.sprite = platformSprite;
        renderer.sortingOrder = 5;
    }

    private void CreateFallbackPlatformVisual(Transform parent, float targetWidth, float targetHeight)
    {
        GameObject visual = new GameObject("FallbackVisual");
        visual.transform.SetParent(parent, false);
        visual.transform.localScale = new Vector3(targetWidth, targetHeight, 1f);

        SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateUnitSprite();
        renderer.color = new Color(0.12f, 0.78f, 0.82f, 1f);
        renderer.sortingOrder = 5;
    }

    private GameObject EnsurePlayer()
    {
        PlayerController existingPlayer = Object.FindFirstObjectByType<PlayerController>();
        if (existingPlayer != null)
        {
            return existingPlayer.gameObject;
        }

        GameObject playerPrefab = GameplayPrefabCatalog.LoadPlayerPrefab();
        if (playerPrefab != null)
        {
            GameObject playerFromPrefab = Instantiate(playerPrefab);
            playerFromPrefab.name = PlayerName;
            playerFromPrefab.transform.position = new Vector3(-6f, -1.8f, 0f);
            return playerFromPrefab;
        }

        GameObject player = new GameObject(PlayerName);
        player.transform.position = new Vector3(-6f, -1.8f, 0f);
        player.transform.localScale = Vector3.one * 4f;

        SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = 10;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 4.6f;
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
        controller.jumpForce = 9f;
        controller.maxFallSpeed = 24f;
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

    private LevelDef GetCurrentLevel()
    {
        if (Levels.AllLevels != null && Levels.AllLevels.Count > 0)
        {
            return Levels.AllLevels[0];
        }

        return null;
    }

    private float GetGroundWidth()
    {
        LevelDef level = GetCurrentLevel();
        if (level != null)
        {
            float levelWidth = level.worldWidth / GameConstants.TILE;
            if (levelWidth > 0f)
            {
                return levelWidth;
            }
        }

        return DefaultGroundWidth;
    }

    private float GetGroundTop()
    {
        return GroundY + (GroundHeight * 0.5f);
    }

    private float TileToWorldX(float tile)
    {
        float leftEdge = -GetGroundWidth() * 0.5f;
        return leftEdge + (tile * PlatformTileSize);
    }

    private float TileToWorldY(float tile)
    {
        return GetGroundTop() + (tile * PlatformTileSize);
    }

    private int GetRotationTurns(PlatformDef platform)
    {
        int turns = platform.rotation ?? 0;
        turns %= 4;
        if (turns < 0)
        {
            turns += 4;
        }

        return turns;
    }

    private Vector2Int[] GetPlatformBlocks(PlatformDef platform)
    {
        if (!Logic.SHAPES.TryGetValue(platform.tetromino, out Vector2Int[] shape))
        {
            return new Vector2Int[0];
        }

        int turns = GetRotationTurns(platform);
        Vector2Int[] rotated = new Vector2Int[shape.Length];

        for (int i = 0; i < shape.Length; i++)
        {
            rotated[i] = Logic.Rotate(shape[i], turns);
        }

        return rotated;
    }

    private Sprite LoadPlatformSprite(Tetromino tetromino)
    {
        return PlatformVisualLibrary.LoadSprite(tetromino);
    }

    private float CreateRepeatedGroundVisuals(Transform parent, Sprite groundSprite, float targetWidth, float targetHeight)
    {
        Vector2 spriteSize = groundSprite.bounds.size;
        if (spriteSize.x <= 0f || spriteSize.y <= 0f)
        {
            return targetWidth;
        }

        float uniformScale = targetHeight / spriteSize.y;
        float tileWidth = spriteSize.x * uniformScale;
        int tileCount = Mathf.Max(1, Mathf.CeilToInt(targetWidth / tileWidth));
        float visualWidth = tileCount * tileWidth;
        float leftEdge = -visualWidth * 0.5f;

        for (int i = 0; i < tileCount; i++)
        {
            GameObject tile = new GameObject($"GroundTile_{i:00}");
            tile.transform.SetParent(parent, false);
            tile.transform.localPosition = new Vector3(leftEdge + (tileWidth * 0.5f) + (i * tileWidth), 0f, 0f);
            tile.transform.localScale = Vector3.one * uniformScale;

            SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
            renderer.sprite = groundSprite;
            renderer.sortingOrder = 0;
        }

        return visualWidth;
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
