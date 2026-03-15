// Auto-generated from levels.ts
// Conversion TypeScript -> C# pour Unity
using System.Collections.Generic;
using UnityEngine;

public static class Levels
{
    // Exemple de structure pour stocker les niveaux
    public static List<LevelDef> AllLevels = new List<LevelDef>
    {
        new LevelDef
        {
            id = "level1",
            world = 1,
            name = "Niveau Test",
            worldWidth = 30 * GameConstants.TILE,
            requiredOrbs = 1,
            spawn = new Vector2(2 * GameConstants.TILE, 10 * GameConstants.TILE),
            portal = new Vector2(25 * GameConstants.TILE, 10 * GameConstants.TILE),
            platforms = new List<PlatformDef>
            {
                new PlatformDef
                {
                    id = "p1",
                    tetromino = Tetromino.I,
                    x = 10,
                    y = 2,
                    type = PlatformType.stable
                },
                new PlatformDef
                {
                    id = "p2",
                    tetromino = Tetromino.O,
                    x = 16,
                    y = 4,
                    type = PlatformType.stable
                },
                new PlatformDef
                {
                    id = "p3",
                    tetromino = Tetromino.T,
                    x = 21,
                    y = 5,
                    type = PlatformType.stable
                },
                new PlatformDef
                {
                    id = "p4",
                    tetromino = Tetromino.L,
                    x = 25,
                    y = 3,
                    rotation = 1,
                    type = PlatformType.stable
                },
                new PlatformDef
                {
                    id = "p5",
                    tetromino = Tetromino.Z,
                    x = 7,
                    y = 6,
                    type = PlatformType.stable
                }
            },
            checkpoints = new List<Checkpoint>(),
            orbs = new List<DataOrb>(),
            enemies = new List<Enemy>(),
            decorations = new List<DecorationDef>()
        }
    };
}
