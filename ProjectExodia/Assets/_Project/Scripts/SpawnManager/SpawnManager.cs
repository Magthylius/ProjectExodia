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
        [SerializeField] private float spawnDistance = 70;
        [SerializeField] private float spawnGap = 4;
        [SerializeField] private float spawnTime;
        [SerializeField] private float yPositionOffset;
        [SerializeField] private bool disableSpawn = false;

        private readonly List<EntityBase> _activeEntities = new();
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
            if (entityPrefabs.Length <= 0) return;
            
            var enemy = Instantiate(entityPrefabs[RandomPrefabIndex()], transform, true);
            enemy.Initialize(_playerManager.Controller.Character.transform);
            var randomGap = Random.Range(-spawnGap, spawnGap);
            
            enemy.transform.position = new Vector3(randomGap, _ySpawnPosition, _tileManager.GetLastSpawnLocation().z);
            _activeEntities.Add(enemy);
        }

        private void DeleteEnemy(EntityBase enemy)
        {
            if (!_activeEntities.Contains(enemy)) return;
            Destroy(enemy.gameObject, 5.0f);
            
            _activeEntities.Remove(enemy);
        }
        
        private int RandomPrefabIndex()
        {
            if (entityPrefabs.Length <= 1) return 0;

            var randomIndex = _lastPrefabIndex;
            
            while (randomIndex == _lastPrefabIndex)
                randomIndex = Random.Range(0, entityPrefabs.Length);

            _lastPrefabIndex = randomIndex;
            return randomIndex;
        }
    }
}
