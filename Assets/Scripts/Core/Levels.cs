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
                    x = 5,
                    y = 12,
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
