using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private GameInitData gameInitData;
        
        [Header("Prefabs")]
        [SerializeField] private PlayerManager playerManagerPrefab;
        [SerializeField] private UIManager uiManagerPrefab;
        [SerializeField] private AudioManager audioManagerPrefab;
        [SerializeField] private SpawnManager spawnManagerPrefab;
        [SerializeField] private TileManager tileManagerPrefab;
        
        private readonly Dictionary<Type, ManagerBase> _managerDictionary = new();
    
        private void Awake()
        {
            CreateManager<GameManager>();
            CreateManager<TimerManager>();
            CreateManager<CameraManager>();
            CreateManager(playerManagerPrefab);
            CreateManager(uiManagerPrefab);
            CreateManager(audioManagerPrefab);
            CreateManager(tileManagerPrefab);
            CreateManager(spawnManagerPrefab);
        }

        public bool TryGetManager<T>(out T outManager) where T : ManagerBase
        {
            var type = typeof(T);
            outManager = null;

            if (!_managerDictionary.TryGetValue(type, out var coreManager)) return false;
        
            outManager = (T)coreManager;
            return true;
        }

        private void CreateManager<T>() where T : ManagerBase
        {
            var classType = typeof(T);
            if (_managerDictionary.ContainsKey(classType)) return;

            var managerObject = new GameObject(classType.Name);
            managerObject.transform.SetParent(transform);
            
            var manager = (ManagerBase)managerObject.AddComponent(classType);
            manager.Initialize(this);
            _managerDictionary.Add(classType, manager);
        }

        private void CreateManager<T>(T managerPrefab) where T : ManagerBase
        {
            var classType = typeof(T);
            if (_managerDictionary.ContainsKey(classType)) return;

            var manager = Instantiate(managerPrefab, transform, true);
            manager.name = classType.Name;
            manager.Initialize(this);
            _managerDictionary.Add(classType, manager);
        }

        public GameInitData GetGameData() => gameInitData;
    }
}
