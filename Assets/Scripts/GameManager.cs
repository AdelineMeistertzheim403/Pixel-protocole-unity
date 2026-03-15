using System.Collections.Generic;
using UnityEngine;

namespace LegacyPrototype
{
    public class GameManager : MonoBehaviour
    {
        public int levelIndex = 0;
        public GameObject platformPrefab;

        private readonly List<GameObject> spawnedPlatforms = new List<GameObject>();

        private void Start()
        {
            LoadLevel(levelIndex);
        }

        public void LoadLevel(int index)
        {
            foreach (var go in spawnedPlatforms)
            {
                if (go != null)
                {
                    Destroy(go);
                }
            }

            spawnedPlatforms.Clear();

            if (Levels.AllLevels.Count == 0)
            {
                Debug.LogWarning("Aucun niveau defini dans Levels.cs");
                return;
            }

            LevelDef level = Levels.AllLevels[Mathf.Clamp(index, 0, Levels.AllLevels.Count - 1)];
            Debug.Log($"Chargement du niveau : {level.id} - {level.name}");

            foreach (var platform in level.platforms)
            {
                var go = Instantiate(platformPrefab);
                go.transform.position = new Vector3(platform.x * GameConstants.TILE, platform.y * GameConstants.TILE, 0);
                go.name = $"Platform_{platform.id}";
                spawnedPlatforms.Add(go);
            }
        }
    }
}
