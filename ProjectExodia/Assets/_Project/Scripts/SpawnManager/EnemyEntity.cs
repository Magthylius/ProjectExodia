using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : MonoBehaviour, ISlappableEntity
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

        public void Initialize(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void PerformSlap()
        {
            effectsPlayer.PlayFeedbacks();
        }

        public string GetName() => gameObject.name;
    }
}
