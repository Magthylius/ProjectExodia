using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectExodia
{
    public class GameContext : MonoBehaviour
    {
        [FormerlySerializedAs("gameData")] [SerializeField] private GameInitData gameInitData;
        
        private readonly Dictionary<Type, ManagerBase> _managerDictionary = new();
    
        private void Awake()
        {
            CreateManager<GameManager>();
            CreateManager<CameraManager>();
            CreateManager<PlayerManager>();
        }

        public bool GetManager<T>(out T outManager) where T : ManagerBase
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

        public GameInitData GetGameData() => gameInitData;
    }
}
