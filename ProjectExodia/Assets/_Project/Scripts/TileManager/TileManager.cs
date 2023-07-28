using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class TileManager : ManagerBase
    {
        private Transform playerTransform;
        private int lastPrefabIndex = 0;
        private List<GameObject> activeTiles;
        private float lastSpawn = 0.0f;
        
        [SerializeField]private GameObject[] tilePrefabs;
        [SerializeField]private float tileLength = 10.0f;
        [SerializeField]private float safeZone = 15.0f;
        [SerializeField]private int maxTileSpawn = 10;
        
        private void Start()
        {
            activeTiles = new List<GameObject>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            
            for (int i = 0; i < maxTileSpawn; i++)
            {
                SpawnTile();
            }
        }
        
        private void Update()
        {
            if (playerTransform.position.z - safeZone > (lastSpawn - maxTileSpawn * tileLength))
            {
                SpawnTile();
                DeleteTile();
            }
        }
        
        private void SpawnTile(int tilePrefabIndex = -1)
        {
            GameObject goTiles = Instantiate(tilePrefabs[RandomPrefabIndex()], transform, true);
            goTiles.transform.position = Vector3.forward * lastSpawn;
            lastSpawn += tileLength;
            activeTiles.Add(goTiles);
        }

        private void DeleteTile()
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }

        private int RandomPrefabIndex()
        {
            if (tilePrefabs.Length <= 1) return 0;

            int randomIndex = lastPrefabIndex;
            
            while (randomIndex == lastPrefabIndex)
                randomIndex = Random.Range(0, tilePrefabs.Length);

            lastPrefabIndex = randomIndex;
            return randomIndex;
        }

        private void ResetTile()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetLastSpawnLocation() => Vector3.forward * lastSpawn;
    }
}
