// Auto-generated from updatePlayer.ts
// Conversion TypeScript -> C# pour Unity
using System;
using UnityEngine;

public static class UpdatePlayer
{
    // Exemple de fonction utilitaire
    public static int GravityDirection(Player player, float now)
    {
        return now < player.gravityInvertedUntil ? -1 : 1;
    }

    // Ajoute ici les autres fonctions de logique du joueur (ApplyMagneticPull, ApplyHackPulse, etc.)
}
