using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[DisallowMultipleComponent]
public class LevelPlatformLayoutAuthoring : MonoBehaviour
{
    private const string PlatformsRootName = "PlatformInstances";

    [SerializeField] private string levelId = "level_scene";
    [SerializeField] private float tileSize = 0.85f;
    [SerializeField] private bool autoSyncInEditor = true;
    [SerializeField] private List<PlatformDef> platforms = new List<PlatformDef>();

    public string LevelId => levelId;
    public float TileSize => tileSize;
    public List<PlatformDef> Platforms => platforms;

    private void OnValidate()
    {
#if UNITY_EDITOR
        tileSize = Mathf.Max(0.01f, tileSize);

        if (!Application.isPlaying && autoSyncInEditor)
        {
            EditorApplication.delayCall -= DelayedSync;
            EditorApplication.delayCall += DelayedSync;
        }
#endif
    }

#if UNITY_EDITOR
    private void DelayedSync()
    {
        if (this == null || gameObject == null || Application.isPlaying)
        {
            return;
        }

        SyncPlatformInstances();
    }
#endif

    [ContextMenu("Sync Platform Instances")]
    public void SyncPlatformInstances()
    {
#if UNITY_EDITOR
        tileSize = Mathf.Max(0.01f, tileSize);

        Transform root = GetOrCreatePlatformsRoot();
        ClearChildren(root);

        foreach (PlatformDef platform in platforms)
        {
            if (platform == null)
            {
                continue;
            }

            GameObject prefab = PlatformPrefabCatalog.LoadPrefab(platform.tetromino, NormalizeRotation(platform.rotation));
            GameObject instance;

            if (prefab != null)
            {
                instance = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, root);
            }
            else
            {
                instance = new GameObject();
                instance.transform.SetParent(root, false);
                PlatformPrefabAuthoring authoring = instance.AddComponent<PlatformPrefabAuthoring>();
                authoring.ApplyPreset(platform.tetromino, NormalizeRotation(platform.rotation), platform.type, tileSize);
                authoring.RebuildPlatform();
            }

            instance.name = $"Platform_{platform.id}";
            instance.transform.localPosition = GetPlatformLocalPosition(platform);

            PlatformSceneInstanceMarker marker = instance.GetComponent<PlatformSceneInstanceMarker>();
            if (marker == null)
            {
                marker = instance.AddComponent<PlatformSceneInstanceMarker>();
            }

            marker.Apply(platform.id, Mathf.RoundToInt(platform.x), Mathf.RoundToInt(platform.y), platform.tetromino, NormalizeRotation(platform.rotation));
        }
#endif
    }

    public PlatformDef AddOrReplacePlatform(int tileX, int tileY, Tetromino tetromino, int rotationTurns, PlatformType platformType)
    {
        PlatformDef existing = FindPlatformAt(tileX, tileY);
        if (existing != null)
        {
            existing.tetromino = tetromino;
            existing.rotation = NormalizeRotation(rotationTurns);
            existing.type = platformType;
            return existing;
        }

        PlatformDef platform = new PlatformDef
        {
            id = GeneratePlatformId(),
            tetromino = tetromino,
            x = tileX,
            y = tileY,
            rotation = NormalizeRotation(rotationTurns),
            type = platformType
        };

        platforms.Add(platform);
        return platform;
    }

    public bool RemovePlatformAt(int tileX, int tileY)
    {
        int index = platforms.FindIndex(platform => platform != null && Mathf.RoundToInt(platform.x) == tileX && Mathf.RoundToInt(platform.y) == tileY);
        if (index < 0)
        {
            return false;
        }

        platforms.RemoveAt(index);
        return true;
    }

    public PlatformDef FindPlatformAt(int tileX, int tileY)
    {
        return platforms.Find(platform => platform != null && Mathf.RoundToInt(platform.x) == tileX && Mathf.RoundToInt(platform.y) == tileY);
    }

    private Transform GetOrCreatePlatformsRoot()
    {
        Transform existing = transform.Find(PlatformsRootName);
        if (existing != null)
        {
            return existing;
        }

        GameObject root = new GameObject(PlatformsRootName);
        root.transform.SetParent(transform, false);
        return root.transform;
    }

    private Vector3 GetPlatformLocalPosition(PlatformDef platform)
    {
        return new Vector3(platform.x * tileSize, platform.y * tileSize, 0f);
    }

    private string GeneratePlatformId()
    {
        int nextIndex = 1;
        HashSet<string> ids = new HashSet<string>();
        foreach (PlatformDef platform in platforms)
        {
            if (platform != null && !string.IsNullOrEmpty(platform.id))
            {
                ids.Add(platform.id);
            }
        }

        while (ids.Contains($"p{nextIndex}"))
        {
            nextIndex++;
        }

        return $"p{nextIndex}";
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

    private static void ClearChildren(Transform root)
    {
        List<GameObject> children = new List<GameObject>();
        for (int index = 0; index < root.childCount; index++)
        {
            children.Add(root.GetChild(index).gameObject);
        }

        foreach (GameObject child in children)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.DestroyObjectImmediate(child);
                continue;
            }
#endif
            Destroy(child);
        }
    }
}
