using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : EntityBase
    {
        [SerializeField] private MMF_Player effectsPlayer;
        [SerializeField] private float despawnOffset = 10.0f;

        private Transform _playerTransform;
        private void Update()
        {
            if (_playerTransform && _playerTransform.position.z - despawnOffset > transform.position.z)
            {
                Destroy(gameObject);
            }
        }

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            if (gameContext.TryGetManager(out PlayerManager manager))
                _playerTransform = manager.Controller.Character.transform;
        }

        public override bool PerformSlap()
        {
            if (!base.PerformSlap()) return false;

            effectsPlayer.PlayFeedbacks();
            return true;
        }
    }
}
