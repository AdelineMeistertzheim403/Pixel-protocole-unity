// Auto-generated from constants.ts
// Conversion TypeScript -> C# pour Unity
using UnityEngine;

public static class GameConstants
{
    public const int TILE = 32;
    public const int VIEWPORT_W = 30 * TILE;
    public const int WORLD_H = 18 * TILE;
    public const int GROUND_Y = WORLD_H - TILE;
    public const float GRAVITY = 1700f;
    public const float SPEED = 240f;
    public const float JUMP = 560f;
    public const float DASH_SPEED = 620f;
    public const int DASH_MS = 150;
    public const float ICE_GROUND_ACCEL = 180f;
    public const float BOOST_JUMP_MULTIPLIER = 1.45f;
    public const float BOOST_OVERCLOCK_MULTIPLIER = 1.62f;
    public const float BOOST_HORIZONTAL_PUSH = 210f;
    public const float CORRUPTED_SPEED_FACTOR = 0.72f;
    public const int CORRUPTED_DURATION_MS = 1300;
    public const int CORRUPTED_DAMAGE_COOLDOWN_MS = 2100;
    public const int GRAVITY_FLIP_DURATION_MS = 2200;
    public const float MAGNETIC_PULL_RADIUS = 250f;
    public const float MAGNETIC_PULL_ACCEL = 480f;
    public const string MOVING_DEFAULT_AXIS = "x";
    public const string MOVING_DEFAULT_PATTERN = "pingpong";
    public const int MOVING_DEFAULT_RANGE_TILES = 4;
    public const float MOVING_DEFAULT_SPEED = 96f;
    public const int WORLD_RENDER_SCALE = 2;
    public const float PLAYER_VISUAL_SCALE = 1.4f;
    public const float ROOKIE_VISUAL_SCALE = 1.65f;
    public const float PULSE_VISUAL_SCALE = 1.55f;
    public const float APEX_VISUAL_SCALE = 1.7f;
    public const int RUN_ANIMATION_FRAME_MS = 300;
    public const float CAMERA_TOP_TRIGGER_RATIO = 0.3f;
    public const float CAMERA_BOTTOM_TRIGGER_RATIO = 0.78f;
    public const int CAMERA_MIN_TOP_PADDING = 3 * TILE;
    public const int DEFAULT_WORLD_TOP_PADDING = 0;

    public const string PLAYER_IDLE_SPRITE = "/sprites_pixel_protocole/pixel/pixel_iddle.png";
    public const string PLAYER_RUN_SPRITE = "/sprites_pixel_protocole/pixel/pixel_run.png";
    public const string PLAYER_JUMP_SPRITE = "/sprites_pixel_protocole/pixel/pixel_jump.png";
}
