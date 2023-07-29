using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : MonoBehaviour, ISlappableEntity
    {
        [SerializeField] private MMF_Player effectsPlayer;
        [SerializeField] private float despawnOffset = 10.0f;
        [SerializeField] private bool allowMultipleSlaps = false;
        
        private Transform _playerTransform;
        private bool _wasSlapped;
        
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

        public bool PerformSlap()
        {
            if (!allowMultipleSlaps && _wasSlapped) return false;
            
            _wasSlapped = true;
            effectsPlayer.PlayFeedbacks();
            return true;
        }

        public bool GetWasSlapped() => _wasSlapped;
        public string GetName() => gameObject.name;
    }
}
