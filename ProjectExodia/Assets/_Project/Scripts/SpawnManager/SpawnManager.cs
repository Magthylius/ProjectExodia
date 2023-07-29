using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class SpawnManager : ManagerBase
    {
        [SerializeField] private EnemyEntity[] enemyEntityPrefabs;
        [SerializeField] private float spawnDistance = 70;
        [SerializeField] private float spawnGap = 4;
        [SerializeField] private float spawnTime;
        [SerializeField] private float yPositionOffset;
        [SerializeField] private bool disableSpawn = false;

        private readonly List<EnemyEntity> _activeEnemies = new();
        private TileManager _tileManager;
        private PlayerManager _playerManager;

        private float _ySpawnPosition;
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
            
            if (!gameContext.TryGetManager(out _playerManager))
            {
                Debug.LogError($"{this}: Failed to get player manager. Disabling {this}'s game object.");
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
            _ySpawnPosition =_tileManager.transform.position.y + yPositionOffset;
        }

        private void SpawnEnemy()
        {
            if (disableSpawn) return;
            if (enemyEntityPrefabs.Length <= 0) return;
            
            var enemy = Instantiate(enemyEntityPrefabs[RandomPrefabIndex()], transform, true);
            enemy.Initialize(_playerManager.Controller.PlayerTransform);
            var randomGap = Random.Range(-spawnGap, spawnGap);
            
            enemy.transform.position = new Vector3(randomGap, _ySpawnPosition, _tileManager.GetLastSpawnLocation().z);
            _activeEnemies.Add(enemy);
        }

        private void DeleteEnemy(EnemyEntity enemy)
        {
            if (!_activeEnemies.Contains(enemy)) return;
            Destroy(enemy.gameObject, 5.0f);
            
            _activeEnemies.Remove(enemy);
        }
        
        private int RandomPrefabIndex()
        {
            if (enemyEntityPrefabs.Length <= 1) return 0;

            var randomIndex = _lastPrefabIndex;
            
            while (randomIndex == _lastPrefabIndex)
                randomIndex = Random.Range(0, enemyEntityPrefabs.Length);

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
