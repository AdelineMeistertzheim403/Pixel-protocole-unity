#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelPlatformLayoutAuthoring))]
public class LevelPlatformLayoutAuthoringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        LevelPlatformLayoutAuthoring layout = (LevelPlatformLayoutAuthoring)target;

        if (GUILayout.Button("Sync Platform Instances"))
        {
            Undo.RecordObject(layout, "Sync platform instances");
            layout.SyncPlatformInstances();
            EditorUtility.SetDirty(layout);
        }

        if (GUILayout.Button("Open Platform Palette"))
        {
            LevelPlatformPaletteWindow.Open(layout);
        }
    }
}

public class LevelPlatformPaletteWindow : EditorWindow
{
    private LevelPlatformLayoutAuthoring layout;
    private Tetromino tetromino = Tetromino.I;
    private int rotationTurns;
    private PlatformType platformType = PlatformType.stable;
    private int tileX;
    private int tileY;

    [MenuItem("Tools/Pixel Protocole/Level Platform Palette")]
    public static void Open()
    {
        GetWindow<LevelPlatformPaletteWindow>("Platform Palette");
    }

    public static void Open(LevelPlatformLayoutAuthoring selectedLayout)
    {
        LevelPlatformPaletteWindow window = GetWindow<LevelPlatformPaletteWindow>("Platform Palette");
        window.layout = selectedLayout;
        window.Show();
    }

    private void OnSelectionChange()
    {
        if (Selection.activeGameObject == null)
        {
            return;
        }

        LevelPlatformLayoutAuthoring selectedLayout = Selection.activeGameObject.GetComponent<LevelPlatformLayoutAuthoring>();
        if (selectedLayout != null)
        {
            layout = selectedLayout;
            Repaint();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scene Layout", EditorStyles.boldLabel);
        layout = (LevelPlatformLayoutAuthoring)EditorGUILayout.ObjectField("Layout", layout, typeof(LevelPlatformLayoutAuthoring), true);

        if (layout == null)
        {
            EditorGUILayout.HelpBox("Selectionne un GameObject avec LevelPlatformLayoutAuthoring dans la scene.", MessageType.Info);

            if (GUILayout.Button("Use Selected Object"))
            {
                TryUseSelectedLayout();
            }

            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Platform", EditorStyles.boldLabel);
        tetromino = (Tetromino)EditorGUILayout.EnumPopup("Tetromino", tetromino);
        rotationTurns = EditorGUILayout.IntSlider("Rotation", rotationTurns, 0, 3);
        platformType = (PlatformType)EditorGUILayout.EnumPopup("Type", platformType);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tile Position", EditorStyles.boldLabel);
        tileX = EditorGUILayout.IntField("Tile X", tileX);
        tileY = EditorGUILayout.IntField("Tile Y", tileY);

        PlatformDef existing = layout.FindPlatformAt(tileX, tileY);
        if (existing != null)
        {
            EditorGUILayout.HelpBox($"Occupied by {existing.id} ({existing.tetromino}, R{NormalizeRotation(existing.rotation)}).", MessageType.Warning);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button(existing == null ? "Add Platform" : "Replace Platform"))
        {
            Undo.RecordObject(layout, existing == null ? "Add platform" : "Replace platform");
            layout.AddOrReplacePlatform(tileX, tileY, tetromino, rotationTurns, platformType);
            EditorUtility.SetDirty(layout);
            layout.SyncPlatformInstances();
        }

        if (GUILayout.Button("Remove Platform At Position"))
        {
            Undo.RecordObject(layout, "Remove platform");
            bool removed = layout.RemovePlatformAt(tileX, tileY);
            if (removed)
            {
                EditorUtility.SetDirty(layout);
                layout.SyncPlatformInstances();
            }
        }

        if (GUILayout.Button("Sync Scene From Layout"))
        {
            layout.SyncPlatformInstances();
            EditorUtility.SetDirty(layout);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Existing Platforms", EditorStyles.boldLabel);

        if (layout.Platforms == null || layout.Platforms.Count == 0)
        {
            EditorGUILayout.HelpBox("Aucune plateforme dans ce layout.", MessageType.None);
            return;
        }

        for (int index = 0; index < layout.Platforms.Count; index++)
        {
            PlatformDef platform = layout.Platforms[index];
            if (platform == null)
            {
                continue;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{platform.id}  {platform.tetromino}  ({platform.x}, {platform.y})  R{NormalizeRotation(platform.rotation)}");

            if (GUILayout.Button("Select", GUILayout.Width(60f)))
            {
                tetromino = platform.tetromino;
                rotationTurns = NormalizeRotation(platform.rotation);
                platformType = platform.type;
                tileX = Mathf.RoundToInt(platform.x);
                tileY = Mathf.RoundToInt(platform.y);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void TryUseSelectedLayout()
    {
        if (Selection.activeGameObject == null)
        {
            return;
        }

        layout = Selection.activeGameObject.GetComponent<LevelPlatformLayoutAuthoring>();
    }

    private static int NormalizeRotation(int? rotationTurns)
    {
        int normalized = rotationTurns ?? 0;
        normalized %= 4;
        if (normalized < 0)
        {
            normalized += 4;
        }

        return normalized;
    }
}
#endif
