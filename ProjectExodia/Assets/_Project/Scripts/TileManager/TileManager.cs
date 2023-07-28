using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class TileManager : ManagerBase
    {
        [SerializeField] private GameObject[] tilePrefabs;
        [SerializeField] private float tileLength = 10.0f;
        [SerializeField] private float safeZone = 15.0f;
        [SerializeField] private int maxTileSpawn = 10;

        private Transform _playerTransform;
        private List<GameObject> _activeTiles;
        
        private int _lastPrefabIndex = 0;
        private float _lastSpawn = 0.0f;
        
        private void Start()
        {
            _activeTiles = new List<GameObject>();

            for (var i = 0; i < maxTileSpawn; i++)
            {
                SpawnTile();
            }
        }
        
        private void Update()
        {
            if (!_playerTransform) return;
            if (_playerTransform.position.z - safeZone > _lastSpawn - maxTileSpawn * tileLength)
            {
                SpawnTile();
                DeleteTile();
            }
        }

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            if (gameContext.TryGetManager<PlayerManager>(out var playerManager))
                _playerTransform = playerManager.Controller.PlayerTransform;
        }

        private void SpawnTile(int tilePrefabIndex = -1)
        {
            var goTiles = Instantiate(tilePrefabs[RandomPrefabIndex()], transform, true);
            goTiles.transform.position = Vector3.forward * _lastSpawn;
            _lastSpawn += tileLength;
            _activeTiles.Add(goTiles);
        }

        private void DeleteTile()
        {
            Destroy(_activeTiles[0]);
            _activeTiles.RemoveAt(0);
        }

        private int RandomPrefabIndex()
        {
            if (tilePrefabs.Length <= 1) return 0;

            int randomIndex = _lastPrefabIndex;
            
            while (randomIndex == _lastPrefabIndex)
                randomIndex = Random.Range(0, tilePrefabs.Length);

            _lastPrefabIndex = randomIndex;
            return randomIndex;
        }

        private void ResetTile()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetLastSpawnLocation() => Vector3.forward * _lastSpawn;
    }
}
