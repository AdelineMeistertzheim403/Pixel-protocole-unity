
// GameManager.cs
// Point d'entrée principal pour le jeu Pixel Protocole dans Unity
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int levelIndex = 0;
    public GameObject platformPrefab;
    public GameObject playerPrefab;

    private readonly List<GameObject> spawnedPlatforms = new List<GameObject>();
    private GameObject spawnedPlayer;

    void Start()
    {
        LoadLevel(levelIndex);
    }

    public void LoadLevel(int index)
    {
        // Nettoyer les plateformes précédentes
        foreach (var go in spawnedPlatforms)
        {
            if (go != null) Destroy(go);
        }
        spawnedPlatforms.Clear();

        // Nettoyer le joueur précédent
        if (spawnedPlayer != null)
        {
            Destroy(spawnedPlayer);
            spawnedPlayer = null;
        }

        if (Levels.AllLevels.Count == 0)
        {
            Debug.LogWarning("Aucun niveau défini dans Levels.cs");
            return;
        }
        LevelDef level = Levels.AllLevels[Mathf.Clamp(index, 0, Levels.AllLevels.Count - 1)];
        Debug.Log($"Chargement du niveau : {level.id} - {level.name}");

        // Instancier les plateformes
        foreach (var platform in level.platforms)
        {
            var go = Instantiate(platformPrefab);
            go.transform.position = new Vector3(platform.x * GameConstants.TILE, platform.y * GameConstants.TILE, 0);
            go.name = $"Platform_{platform.id}";
            spawnedPlatforms.Add(go);
        }

        // Instancier le joueur à la position de spawn
        if (playerPrefab != null)
        {
            spawnedPlayer = Instantiate(playerPrefab);
            spawnedPlayer.transform.position = new Vector3(level.spawn.x, level.spawn.y, 0);
            AttachCameraToPlayer(spawnedPlayer.transform);
        }
    }

    private void AttachCameraToPlayer(Transform playerTransform)
    {
        Camera camera = Camera.main;
        if (camera == null || playerTransform == null)
        {
            return;
        }

        CameraFollow2D follow = camera.GetComponent<CameraFollow2D>();
        if (follow == null)
        {
            follow = camera.gameObject.AddComponent<CameraFollow2D>();
        }

        follow.offset = new Vector3(0f, 1.1f, -10f);
        follow.smoothTime = 0.1f;
        follow.SetTarget(playerTransform);
    }
}
