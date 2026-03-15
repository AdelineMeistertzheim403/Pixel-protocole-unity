// Auto-generated from dialogue.ts
// Conversion TypeScript -> C# pour Unity
using System;
using System.Collections.Generic;

public enum PlatformerEvent
{
    level_start,
    player_jump_chain,
    player_fall,
    player_collect_orb,
    player_collect_many_orbs,
    player_near_death,
    player_escape,
    player_kill_robot,
    player_kill_apex,
    player_speedrun,
    player_idle,
    player_fail_jump,
    player_finish_level,
    player_secret_found,
    player_glitch_power
}

public class PixelProtocolChatLine
{
    public EnemyKind speaker;
    public string name;
    public string color;
    public string text;
    public int at;
}

public static class Dialogue
{
    // Ajoute ici les dictionnaires de dialogues et couleurs par EnemyKind
    // Exemple : public static Dictionary<EnemyKind, string> Names = ...
}
