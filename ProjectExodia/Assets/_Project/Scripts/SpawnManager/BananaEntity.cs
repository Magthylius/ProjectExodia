using System;
using UnityEngine;

namespace ProjectExodia
{
    public class BananaEntity : EntityBase
    {
        [SerializeField] private float despawnOffset = 10.0f;
        private PlayerCharacter _playerCharacter;
        
        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            if (gameContext.TryGetManager(out PlayerManager manager))
                _playerCharacter = manager.Controller.Character;
        }
        
        private void Update()
        {
            if (_playerCharacter && _playerCharacter.transform.position.z - despawnOffset > transform.position.z)
            {
                Destroy(gameObject);
            }
        }

        public override void PerformCollision()
        {
            Destroy(gameObject);
        }
    }
}