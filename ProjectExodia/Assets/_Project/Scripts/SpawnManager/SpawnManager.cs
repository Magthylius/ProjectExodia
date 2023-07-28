using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class SpawnManager : ManagerBase
    {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private float spawnDistance = 70;
        [SerializeField] private float spawnGap = 4;
        [SerializeField] private float spawnTime;
        [SerializeField] private float yPositionOffset;

        private readonly List<GameObject> _activeEnemies = new();
        private TileManager _tileManager;
        
        private float _timerElapsed;
        private int _lastPrefabIndex = 0;

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);

            if (!gameContext.TryGetManager(out _tileManager))
            {
                Debug.LogError($"{this}: Failed to get tile manager. Disabling {this}'s game object.");
                gameObject.SetActive(false);
                return;
            }

            if (!GameContext.TryGetManager<TimerManager>(out var timerManager))
            {
                Debug.LogError($"{this}: Failed to get timer manager. Disabling {this}'s game object.");
                gameObject.SetActive(false);
                return;
            }
            
            timerManager.CreateTimer(SpawnEnemy, spawnTime, true);
        }

        private void SpawnEnemy()
        {
            var goEnemy = Instantiate(enemyPrefabs[RandomPrefabIndex()], transform, true);
            var randomGap = Random.Range(-spawnGap, spawnGap);

            goEnemy.transform.position = new Vector3(randomGap, (_tileManager.transform.position.y + yPositionOffset),
                _tileManager.GetLastSpawnLocation().z);
            _activeEnemies.Add(goEnemy);
        }

        private void DeleteEnemy(GameObject pEnemyToRemove)
        {
            var enemyToRemove = _activeEnemies.Find(obj => obj == pEnemyToRemove);
            if (enemyToRemove == null) return;
            Destroy(enemyToRemove, 5.0f);
            _activeEnemies.Remove(pEnemyToRemove);
        }
        
        private int RandomPrefabIndex()
        {
            if (enemyPrefabs.Length <= 1) return 0;

            var randomIndex = _lastPrefabIndex;
            
            while (randomIndex == _lastPrefabIndex)
                randomIndex = Random.Range(0, enemyPrefabs.Length);

            _lastPrefabIndex = randomIndex;
            return randomIndex;
        }

        private void OnDrawGizmosSelected()
        {
            var tileManagerTransform = _tileManager.transform;
            var position = tileManagerTransform.position;
            
            var minBoundaryLocation = new Vector3((position.x + spawnGap), position.y, position.z);
            var maxBoundaryLocation = new Vector3((position.x - spawnGap), position.y, position.z);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(minBoundaryLocation, Vector3.one);
            Gizmos.DrawWireCube(maxBoundaryLocation, Vector3.one);
        }
    }
}
