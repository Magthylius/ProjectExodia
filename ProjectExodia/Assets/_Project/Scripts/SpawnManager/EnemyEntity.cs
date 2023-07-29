using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : EntityBase
    {
        [SerializeField] private MMF_Player effectsPlayer;
        [SerializeField] private float despawnOffset = 10.0f;

        private PlayerCharacter _playerCharacter;

        private void Update()
        {
            if (_playerCharacter && _playerCharacter.transform.position.z - despawnOffset > transform.position.z)
            {
                Destroy(gameObject);
            }
        }

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            if (gameContext.TryGetManager(out PlayerManager manager))
                _playerCharacter = manager.Controller.Character;
        }

        public override bool PerformSlap()
        {
            if (!base.PerformSlap()) return false;

            effectsPlayer.PlayFeedbacks();
            return true;
        }

        public override void PerformCollision()
        {
            if (_playerCharacter) _playerCharacter.TakeHordeDamage();
            Destroy(gameObject);
        }
    }
}
