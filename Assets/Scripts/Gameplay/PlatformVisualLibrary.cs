using System.Collections.Generic;
using UnityEngine;

public static class PlatformVisualLibrary
{
    private static readonly Dictionary<Tetromino, string> TetrominoSpritePaths = new Dictionary<Tetromino, string>
    {
        { Tetromino.I, "Assets/Sprites/PixelProtocol/decorations/dec_001_tetromino_i_solid.svg" },
        { Tetromino.O, "Assets/Sprites/PixelProtocol/decorations/dec_004_tetromino_o_solid.svg" },
        { Tetromino.T, "Assets/Sprites/PixelProtocol/decorations/dec_007_tetromino_t_solid.svg" },
        { Tetromino.L, "Assets/Sprites/PixelProtocol/decorations/dec_010_tetromino_l_solid.svg" },
        { Tetromino.J, "Assets/Sprites/PixelProtocol/decorations/dec_013_tetromino_j_solid.svg" },
        { Tetromino.S, "Assets/Sprites/PixelProtocol/decorations/dec_016_tetromino_s_solid.svg" },
        { Tetromino.Z, "Assets/Sprites/PixelProtocol/decorations/dec_019_tetromino_z_solid.svg" },
    };

    public static string GetSpriteAssetPath(Tetromino tetromino)
    {
        return TetrominoSpritePaths.TryGetValue(tetromino, out string assetPath) ? assetPath : null;
    }

    public static Sprite LoadSprite(Tetromino tetromino)
    {
        string assetPath = GetSpriteAssetPath(tetromino);
        return string.IsNullOrEmpty(assetPath) ? null : EditorAssetSpriteLoader.LoadSprite(assetPath);
    }
}
