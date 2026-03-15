// Auto-generated from levelBuilders.ts
// Conversion TypeScript -> C# pour Unity
using System;
using System.Collections.Generic;
using UnityEngine;

public static class LevelBuilders
{
    public static DataOrb OrbOnTile(string id, int tileX, int tileY)
    {
        return new DataOrb {
            id = id,
            x = tileX * GameConstants.TILE + 7,
            y = tileY * GameConstants.TILE - 20,
            affinity = DataOrbAffinity.standard
        };
    }

    // Ajoute ici d'autres méthodes utilitaires pour générer plateformes, ennemis, etc.
    // Exemples : SkillOrbOnTile, PlatformPattern, EnemyPattern, etc.
}
