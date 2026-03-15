using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteAnimator : MonoBehaviour
{
    private const string SpriteFolder = "Sprites/pixel";
    private const string IdleSheet = "pixel_iddle";
    private const string RunSheet = "pixel_run";
    private const string JumpSheet = "pixel_jump";
    private const string IdleSpriteName = "pixel_iddle_7";
    private const string RunSpriteName = "pixel_run_0";
    private const string JumpSpriteName = "pixel_jump_0";

    private PlayerController controller;
    private SpriteRenderer spriteRenderer;

    private Sprite idleSprite;
    private Sprite runSprite;
    private Sprite jumpSprite;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        idleSprite = LoadSprite(IdleSheet, IdleSpriteName);
        runSprite = LoadSprite(RunSheet, RunSpriteName) ?? idleSprite;
        jumpSprite = LoadSprite(JumpSheet, JumpSpriteName) ?? idleSprite;

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
            spriteRenderer.sprite = runSprite;
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }

        spriteRenderer.flipX = !controller.FacingRight;
    }

    private Sprite LoadSprite(string sheetName, string spriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"{SpriteFolder}/{sheetName}");
        Sprite sprite = sprites.FirstOrDefault(candidate => candidate.name == spriteName);

        if (sprite != null)
        {
            return sprite;
        }

        if (sprites.Length > 0)
        {
            return sprites.OrderBy(candidate => candidate.rect.width * candidate.rect.height).Last();
        }

        return CreateFallbackSprite();
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
