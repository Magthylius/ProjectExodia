using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class SpawnManager : ManagerBase
    {
        [SerializeField] private EnemyEntity[] enemyEntityPrefabs;
        [SerializeField] private SpawnPattern[] spawnPatterns;
        [SerializeField] private float spawnDistanceBetweenEnemy = 70;
        [SerializeField] private float spawnYAxisGap = 4;
        [SerializeField] private float initialSpawnDistance;
        [SerializeField] private float yPositionOffset;
        [SerializeField] private float randomOffsetSpawn = 0.0f;
        [SerializeField] private bool disableSpawn = false;
        [SerializeField] private float marginBoundSize = 1.0f;
        
        private readonly List<EnemyEntity> _activeEnemies = new();
        private TileManager _tileManager;
        private PlayerManager _playerManager;

        private float _ySpawnPosition;
        private float _timerElapsed;
        private int _lastPrefabIndex = 0;

        private float _meshBoundSize;
        private float _snapshotDistance;
        private int _currentSpawnIndex = 0;
        private int _randomSpawnPatternPrefab = 0;
        private float _pawnDistanceFromGap;

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
            
            // timerManager.CreateTimer(SpawnEnemy, spawnTime, true);
            _ySpawnPosition = _tileManager.transform.position.y + yPositionOffset;
            _snapshotDistance = _playerManager.Controller.PlayerTransform.position.z;
            _snapshotDistance = initialSpawnDistance;
            _meshBoundSize = _tileManager.GetTileWidth();
        }

        private void Update()
        {
            if (_playerManager.Controller.PlayerTransform.position.z > _snapshotDistance + spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].distanceStamp)
            {
                SpawnEnemy();
                _snapshotDistance += spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].distanceStamp;
                _currentSpawnIndex++;
                if (spawnPatterns[_randomSpawnPatternPrefab].patternList.Count <= _currentSpawnIndex)
                {
                    _randomSpawnPatternPrefab = RandomPrefabIndex(spawnPatterns);
                    _currentSpawnIndex = 0;
                    _snapshotDistance += spawnPatterns[_randomSpawnPatternPrefab].spawnDistanceBetweenEachPattern;
                }
            }
        }

        private void SpawnEnemy()
        {
            if (disableSpawn) return;
            if (enemyEntityPrefabs.Length <= 0) return;
            
            var enemy = Instantiate(enemyEntityPrefabs[RandomPrefabIndex(enemyEntityPrefabs)], transform, true);
            enemy.Initialize(_playerManager.Controller.PlayerTransform);
            
            var spawnIndex = spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].spawnIndex;
            var segment = (_meshBoundSize - marginBoundSize) / 10;
            var xAxisLocation = Random.Range(-randomOffsetSpawn, randomOffsetSpawn);
            var zAxisLocation = _snapshotDistance + spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].distanceStamp +
                                spawnDistanceBetweenEnemy;
            
            xAxisLocation += segment * spawnIndex;
            
            enemy.transform.position = new Vector3(xAxisLocation, _ySpawnPosition, zAxisLocation);
            _activeEnemies.Add(enemy);
        }

        private void DeleteEnemy(EnemyEntity enemy)
        {
            if (!_activeEnemies.Contains(enemy)) return;
            Destroy(enemy.gameObject, 5.0f);
            
            _activeEnemies.Remove(enemy);
        }
        
        private int RandomPrefabIndex<T>(IReadOnlyCollection<T> array)
        {
            if (array.Count <= 1) return 0;

            var randomIndex = _lastPrefabIndex;
            
            while (randomIndex == _lastPrefabIndex)
                randomIndex = Random.Range(0, array.Count);

            _lastPrefabIndex = randomIndex;
            return randomIndex;
        }

        private void OnDrawGizmosSelected()
        {
            var tileManagerTransform = _tileManager.transform;
            var position = tileManagerTransform.position;
            
            var minBoundaryLocation = new Vector3((position.x + spawnYAxisGap), position.y, position.z);
            var maxBoundaryLocation = new Vector3((position.x - spawnYAxisGap), position.y, position.z);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(minBoundaryLocation, Vector3.one);
            Gizmos.DrawWireCube(maxBoundaryLocation, Vector3.one);
        }
    }
}
