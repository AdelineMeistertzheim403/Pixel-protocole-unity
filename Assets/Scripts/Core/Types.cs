
// Auto-generated from types.ts
// Conversion TypeScript -> C# pour Unity
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Tetromino { I, O, T, L, J, S, Z }

public enum BuiltinDecorationType {
    tetromino_I, tetromino_T, tetromino_L, tetromino_Z, tetromino_O, tetromino_S, tetromino_J,
    tetromino_fragment, tetromino_outline, tetromino_glow_block, stacked_tetromino_blocks, tetromino_shadow,
    broken_tetromino, mini_tetromino_cluster, tetromino_neon_border, data_line, data_nodes, energy_pillar,
    data_beam, energy_core, horizontal_data_stream, vertical_data_stream, data_pulse_nodes, circuit_cross,
    data_arrow, packet_squares, network_hub, data_ladder, digital_crosshair, signal_beam, pixel_glitch,
    broken_pixels, glitch_bar, data_noise, fragment_blocks, glitch_stripes, glitch_fragments, broken_grid,
    pixel_noise, corrupted_blocks, glitch_diagonal, data_crack, static_bar, corruption_wave, broken_pixels_cluster,
    teleport_ring, portal_grid, network_triangle, data_hub, node_cluster, server_block, circuit_board, ai_eye,
    code_panel, matrix_block, ai_eye_large, core_processor, server_rack, ai_triangle, neural_nodes, digital_chip,
    circuit_hub, core_ring, data_scanner, processor_grid, grid_background, vertical_grid, floating_squares,
    energy_arcs, digital_wave, horizon_grid, neon_arc, floating_squares_cluster, background_circuit, data_skyline,
    pixel_star, neon_rectangle, digital_tunnel, data_pulse, wave_grid
}

public enum DecorationLayer { far, mid, near }
public enum DecorationAnimation { none, pulse, flow, glitch }
public enum PlatformType {
    stable, unstable, moving, rotating, glitch, bounce, boost, corrupted, magnetic, ice, gravity, grapplable, armored, hackable
}
public enum EnemyKind { rookie, pulse, apex }
public enum PixelSkill {
    DATA_GRAPPLE, OVERJUMP, PHASE_SHIFT, PULSE_SHOCK, OVERCLOCK_MODE, TIME_BUFFER, PLATFORM_SPAWN
}
public enum DataOrbAffinity { standard, blue, red, green, purple }

[Serializable]
public class AbilityFlags {
    public bool doubleJump;
    public int extraAirJumps;
    public bool airDash;
    public bool hackWave;
    public bool shield;
    public bool overjump;
    public bool dataGrapple;
    public bool phaseShift;
    public bool pulseShock;
    public bool overclockMode;
    public bool timeBuffer;
    public bool platformSpawn;
}

[Serializable]
public class PlatformDef {
    public string id;
    public Tetromino tetromino;
    public float x, y;
    public int? rotation;
    public PlatformType type;
    public int? rotateEveryMs;
    public string moveAxis;
    public string movePattern;
    public int? moveRangeTiles;
    public float? moveSpeed;
}

[Serializable]
public class DecorationDef {
    public string id;
    public string type;
    public float x, y;
    public float width, height;
    public float? rotation;
    public float? opacity;
    public string color;
    public string colorSecondary;
    public DecorationLayer? layer;
    public DecorationAnimation? animation;
    public bool? flipX;
    public bool? flipY;
}

[Serializable]
public class DataOrb {
    public string id;
    public float x, y;
    public DataOrbAffinity? affinity;
    public PixelSkill? grantsSkill;
    public bool? taken;
}

[Serializable]
public class Checkpoint {
    public string id;
    public float x, y;
    public float spawnX, spawnY;
    public bool? activated;
}

[Serializable]
public class Enemy {
    public string id;
    public EnemyKind kind;
    public float x, y;
    public float vx;
    public float minX, maxX;
    public float stunnedUntil;
}

[Serializable]
public class LevelDef {
    public string id;
    public int world;
    public string name;
    public float worldWidth;
    public float? worldHeight;
    public float? worldTopPadding;
    public string worldTemplateId;
    public int requiredOrbs;
    public Vector2 spawn;
    public Vector2 portal;
    public List<PlatformDef> platforms;
    public List<Checkpoint> checkpoints;
    public List<DataOrb> orbs;
    public List<Enemy> enemies;
    public List<DecorationDef> decorations;
}

[Serializable]
public class WorldTemplate {
    public string id;
    public string name;
    public float worldWidth;
    public float? worldHeight;
    public float? worldTopPadding;
    public List<DecorationDef> decorations;
    public string updatedAt;
}

[Serializable]
public class Player {
    public float x, y, w, h;
    public float vx, vy;
    public int facing;
    public bool grounded;
    public int jumpsLeft;
    public int hp;
    public float dashUntil, dashCooldownUntil, invulnUntil, phaseShiftUntil, phaseShiftCooldownUntil, overclockUntil, overclockCooldownUntil, pulseShockCooldownUntil, timeBufferCooldownUntil, platformSpawnCooldownUntil, grappleUntil, grappleCooldownUntil;
    public float? grappleTargetX, grappleTargetY, grappleLandY;
    public string grapplePlatformId;
    public string grappleAttachSide;
    public string groundPlatformId;
    public PlatformType? groundedSurface;
    public float gravityInvertedUntil, corruptedUntil, corruptedDamageCooldownUntil;
}

[Serializable]
public class RuntimePlatform : PlatformDef {
    public int currentRotation;
    public bool active;
    public float unstableWakeAt, unstableDropAt, hackedUntil, nextRotateAt;
    public float? expiresAt;
    public bool temporary;
    public float moveOriginX, moveOriginY, moveProgress;
    public int moveDirection;
    public float prevX, prevY;
}

[Serializable]
public class GrappleAnchor {
    public float x, y, landY;
    public string platformId;
    public string attachSide;
}

[Serializable]
public class PlayerHistoryEntry {
    public float at, x, y, vx, vy;
    public int facing;
    public bool grounded;
    public int jumpsLeft, hp;
}

[Serializable]
public class GameRuntime {
    public float startedAt;
    public Player player;
    public List<RuntimePlatform> platforms;
    public List<Checkpoint> checkpoints;
    public Vector2 respawn;
    public List<DataOrb> orbs;
    public List<Enemy> enemies;
    public float cameraX, cameraY;
    public int collected;
    public List<PlayerHistoryEntry> history;
    public string status;
    public string message;
}

[Serializable]
public class Rect {
    public float x, y, w, h;
    public string platformId;
    public PlatformType? type;
}

[Serializable]
public class InputSnapshot {
    public bool left, right, up, down;
    public bool wantJump, wantDash, wantHack, wantGrapple, wantPhaseShift, wantPulseShock, wantOverclock, wantTimeBuffer, wantPlatformSpawn, wantRespawn;
}

// Note: Les types Template string TypeScript (ex: `svg_pack:${string}`) sont convertis en string en C#.
// Les types optionnels sont représentés par des types nullable ou des champs string/class nullables.
// Les Vector2 nécessitent UnityEngine; sinon remplacer par une classe maison.
