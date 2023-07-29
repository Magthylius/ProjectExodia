using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class PlayerCharacter : MonoBehaviour
    {
        public delegate void CharacterEvent();
        public event CharacterEvent OnBananaPickup;

        [SerializeField] private Animator buttBananaAnimator;
        
        [SerializeField] private float bananaSlapDuration = 10f;

        private PlayerController _playerController;
        private HashSet<EntityBase> _entitiesInRange = new();

        private Coroutine _bananaSlapRoutine;
        private bool _isInBananaSlapMode = false;
        
        public bool IsInBananaSlapMode
        {
            get => _isInBananaSlapMode;
            set
            {
                Debug.Log($"Banana slap mode: {value}");
                _isInBananaSlapMode = value;

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
            _entitiesInRange.Add(entity);
            //Debug.Log($"Trigger entry {entity.GetName()}");

            if (_isInBananaSlapMode)
            {
                //! TODO: do something
                PerformSlap();
                return;
            }
            
            entity.PerformCollision();
        }

        private void OnTriggerExit(Collider other)
        {
            var entity = other.transform.GetComponent<EntityBase>();
            if (!entity || !_entitiesInRange.Contains(entity)) return;
            _entitiesInRange.Remove(entity);
            
            //Debug.Log($"Trigger exit {entity.GetName()}");
        }

        public void Initialize(PlayerController controller)
        {
            _playerController = controller;
        }

        public void PerformSlap()
        {
            //Debug.Log($"Slap performed, entities {_entitiesInRange.Count}");
            foreach (var entity in _entitiesInRange)
            {
                _playerController.SlapEntity(entity);
            }
            
            _entitiesInRange.Clear();
        }

        public void OnBananaCountChanged(int bananaCount)
        {
            buttBananaAnimator.SetInteger("BananaCount", bananaCount);
        }
        
        public void TakeHordeDamage()
        {
            ScoreData.IncreaseMultiplier(-ScoreData.MULTIPLIER_BASE_INCREMENT);
        }
    }
}
