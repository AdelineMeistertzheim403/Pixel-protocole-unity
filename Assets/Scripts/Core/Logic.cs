// Auto-generated from logic.ts
// Conversion TypeScript -> C# pour Unity
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Logic
{
    public static readonly Dictionary<Tetromino, Vector2Int[]> SHAPES = new()
    {
        { Tetromino.I, new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) } },
        { Tetromino.O, new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) } },
        { Tetromino.T, new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1) } },
        { Tetromino.L, new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1) } },
        { Tetromino.J, new[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) } },
        { Tetromino.S, new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1) } },
        { Tetromino.Z, new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) } },
    };

    public static float Clamp(float value, float min, float max)
    {
        return Mathf.Max(min, Mathf.Min(max, value));
    }

    public static Vector2Int Rotate(Vector2Int point, int turns)
    {
        var p = point;
        for (int i = 0; i < turns; i++)
            p = new Vector2Int(-p.y, p.x);
        return p;
    }

    public static bool RectIntersects(Rect a, Rect b)
    {
        return a.x < b.x + b.w && a.x + a.w > b.x && a.y < b.y + b.h && a.y + a.h > b.y;
    }

    public static bool OverlapX(float aX, float aW, float bX, float bW)
    {
        return aX < bX + bW && aX + aW > bX;
    }

    public static float? FindSupportTop(List<Rect> blocks, float x, float y, float w, float h)
    {
        float feetY = y + h;
        float? best = null;
        foreach (var block in blocks)
        {
            if (!OverlapX(x, w, block.x, block.w)) continue;
            if (Mathf.Abs(feetY - block.y) > 8) continue;
            if (best == null || block.y < best) best = block.y;
        }
        return best;
    }

    public static List<Rect> PlatformBlocks(RuntimePlatform platform)
    {
        if (!platform.active) return new List<Rect>();
        var baseShape = SHAPES[platform.tetromino];
        var blocks = new List<Rect>();
        foreach (var block in baseShape)
        {
            var p = Rotate(block, platform.currentRotation);
            blocks.Add(new Rect
            {
                x = (platform.x + p.x) * GameConstants.TILE,
                y = (platform.y + p.y) * GameConstants.TILE,
                w = GameConstants.TILE,
                h = GameConstants.TILE,
                platformId = platform.id,
                type = platform.type
            });
        }
        return blocks;
    }

    // ... (Pour la suite, ajouter les autres fonctions selon le même principe)
}
