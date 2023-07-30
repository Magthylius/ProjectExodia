using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class SpawnManager : ManagerBase
    {
        [FormerlySerializedAs("enemyEntityPrefabs")] 
        [SerializeField] private EntityBase[] entityPrefabs;
        [SerializeField] private SpawnPattern[] spawnPatterns;
        [SerializeField] private float spawnDistanceBetweenEnemy = 70;
        [SerializeField] private float initialSpawnDistance;
        [SerializeField] private float yPositionOffset;
        [SerializeField] private float randomOffsetSpawn = 0.0f;
        [SerializeField] private float marginBoundSize = 1.0f;

        private readonly List<EntityBase> _activeEntities = new();
        private TileManager _tileManager;
        private PlayerManager _playerManager;

        private bool _disableSpawn = true;
        private float _ySpawnPosition;
        private float _timerElapsed;
        private int _lastEntityIndex = 0;
        private int _lastPatternIndex = 0;

        private float _meshBoundSize;
        private float _snapshotDistance;
        private int _currentSpawnIndex = 0;
        private int _randomSpawnPatternPrefab = 0;
        private float _pawnDistanceFromGap;

        private void OnEnable()
        {
            LevelTransitionManager.OnCountryChange += UpdateEntityPack;
        }

        private void OnDisable()
        {
            LevelTransitionManager.OnCountryChange -= UpdateEntityPack;
        }

        void UpdateEntityPack(CountryPack country)
        {
            entityPrefabs = country.Enemies;
        }
        
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
            _snapshotDistance = _playerManager.Controller.Character.transform.position.z;
            _snapshotDistance = initialSpawnDistance;
            _meshBoundSize = _tileManager.GetTileWidth();
        }

        private void Update()
        {
            if (entityPrefabs == null)
                return;
            
            if (_playerManager.Controller.Character.transform.position.z > _snapshotDistance + spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].distanceStamp)
            {
                SpawnEnemy();
                _snapshotDistance += spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].distanceStamp;
                _currentSpawnIndex++;
                if (spawnPatterns[_randomSpawnPatternPrefab].patternList.Count <= _currentSpawnIndex)
                {
                    _randomSpawnPatternPrefab = RandomPrefabIndex(spawnPatterns);
                    
                    _currentSpawnIndex = 0;
                    _snapshotDistance += spawnPatterns[_randomSpawnPatternPrefab].spawnDistanceBetweenEachPattern;
                    _lastPatternIndex = _randomSpawnPatternPrefab;
                }
            }
        }

        private void SpawnEnemy()
        {
            Debug.Log($"Spawning {_disableSpawn}, {entityPrefabs.Length}");
            if (_disableSpawn) return;
            if (entityPrefabs.Length <= 0) return;
            
            var enemy = Instantiate(entityPrefabs[RandomPrefabIndex(entityPrefabs)], transform, true);
            enemy.Initialize(GameContext);
            
            var spawnIndex = spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].spawnIndex;
            var segment = (_meshBoundSize - marginBoundSize) / 10;
            var xAxisLocation = Random.Range(-randomOffsetSpawn, randomOffsetSpawn);
            var zAxisLocation = _snapshotDistance + spawnPatterns[_randomSpawnPatternPrefab].patternList[_currentSpawnIndex].distanceStamp +
                                spawnDistanceBetweenEnemy;
            
            xAxisLocation += segment * spawnIndex;
            
            enemy.transform.position = new Vector3(xAxisLocation, _ySpawnPosition, zAxisLocation);
            _activeEntities.Add(enemy);
        }

        private void DeleteEnemy(EntityBase enemy)
        {
            if (!_activeEntities.Contains(enemy)) return;
            Destroy(enemy.gameObject, 5.0f);
            
            _activeEntities.Remove(enemy);
        }
        
        private int RandomPrefabIndex(EntityBase[] array)
        {
            if (array.Length <= 1) return 0;

            var randomIndex = _lastEntityIndex;
            
            while (randomIndex == _lastEntityIndex)
                randomIndex = Random.Range(0, array.Length);

            _lastEntityIndex = randomIndex;
            return randomIndex;
        }
        
        private int RandomPrefabIndex(SpawnPattern[] array)
        {
            if (array.Length <= 1) return 0;

            var randomIndex = _lastPatternIndex;
            
            while (randomIndex == _lastPatternIndex)
                randomIndex = Random.Range(0, array.Length);

            _lastPatternIndex = randomIndex;
            return randomIndex;
        }
        
        public void SetSpawnEntities(bool spawnEntities) => _disableSpawn = !spawnEntities;
    }
}
