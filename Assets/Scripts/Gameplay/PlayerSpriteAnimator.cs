using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteAnimator : MonoBehaviour
{
    private const string IdleTexturePath = "Assets/Sprites/pixel/pixel_iddle.png";
    private const string RunTexturePath = "Assets/Sprites/pixel/pixel_run.png";
    private const string JumpTexturePath = "Assets/Sprites/pixel/pixel_jump.png";

    private PlayerController controller;
    private SpriteRenderer spriteRenderer;

    private Sprite idleSprite;
    private Sprite runSprite;
    private Sprite jumpSprite;

    [SerializeField] private float runAnimationSpeed = 7f;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        idleSprite = LoadSingleSprite(IdleTexturePath);
        runSprite = LoadSingleSprite(RunTexturePath) ?? idleSprite;
        jumpSprite = LoadSingleSprite(JumpTexturePath) ?? idleSprite;

        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    void LateUpdate()
    {
        if (controller == null || spriteRenderer == null)
        {
            return;
        }

        if (!controller.IsGrounded)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else if (controller.IsMoving)
        {
            spriteRenderer.sprite = GetRunFrame();
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }

        spriteRenderer.flipX = !controller.FacingRight;
    }

    private Sprite GetRunFrame()
    {
        if (idleSprite == null || runSprite == null)
        {
            return idleSprite;
        }

        int frameIndex = Mathf.FloorToInt(Time.time * runAnimationSpeed) % 2;
        return frameIndex == 0 ? idleSprite : runSprite;
    }

    private Sprite LoadSingleSprite(string texturePath)
    {
        Texture2D texture = EditorAssetSpriteLoader.LoadTexture(texturePath);
        if (texture == null)
        {
            return CreateFallbackSprite();
        }

        float pixelsPerUnit = texture.height * Mathf.Max(transform.lossyScale.y, 1f);
        return Sprite.Create(
            texture,
            new UnityEngine.Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.05f),
            pixelsPerUnit);
    }

    private Sprite CreateFallbackSprite()
    {
        Texture2D texture = new Texture2D(16, 24, TextureFormat.RGBA32, false);
        Color clear = new Color(0f, 0f, 0f, 0f);
        Color body = new Color(0.52f, 0.95f, 0.82f, 1f);
        Color visor = new Color(0.09f, 0.15f, 0.24f, 1f);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, clear);
            }
        }

        for (int y = 3; y < 21; y++)
        {
            for (int x = 3; x < 13; x++)
            {
                texture.SetPixel(x, y, body);
            }
        }

        for (int y = 12; y < 17; y++)
        {
            for (int x = 4; x < 12; x++)
            {
                texture.SetPixel(x, y, visor);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        return Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.05f), 16f);
    }
}
