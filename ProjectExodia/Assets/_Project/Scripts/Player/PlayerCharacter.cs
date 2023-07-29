using System.Collections;
using UnityEngine;

namespace ProjectExodia
{
    public class PlayerCharacter : MonoBehaviour
    {
        public delegate void CharacterEvent();
        public event CharacterEvent OnBananaPickup;

        [SerializeField] private SphereCollider bananaSlapCollider;
        [SerializeField] private float bananaSlapDuration = 10f;

        private PlayerController _playerController;
        
        private Coroutine _bananaSlapRoutine;
        private bool _isInBananaSlapMode = false;
        public bool IsInBananaSlapMode
        {
            get => _isInBananaSlapMode;
            set
            {
                Debug.Log($"Banana slap mode: {value}");
                _isInBananaSlapMode = value;
                bananaSlapCollider.enabled = value;

                if (!value) return;

                IEnumerator SlapRoutine()
                {
                    yield return new WaitForSeconds(bananaSlapDuration);
                    IsInBananaSlapMode = false;
                }
                    
                if (_bananaSlapRoutine != null) StopCoroutine(_bananaSlapRoutine);
                _bananaSlapRoutine = StartCoroutine(SlapRoutine());
            }
        }
        
        private void OnEnable()
        {
            IsInBananaSlapMode = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var entity = other.transform.GetComponent<EntityBase>();
            if (!entity) return;

            if (_isInBananaSlapMode)
            {
                //! TODO: do something
                entity.PerformSlap();
                _playerController.SlapEntity(entity);
                return;
            }
            
            entity.PerformCollision();
            
            if (entity is BananaEntity)
            {
                OnBananaPickup?.Invoke();
            }
            else if (entity is EnemyEntity)
            {
                TakeHordeDamage();
            }
        }

        public void Initialize(PlayerController controller)
        {
            _playerController = controller;
        }

        public void TakeHordeDamage()
        {
            
        }
    }
}
