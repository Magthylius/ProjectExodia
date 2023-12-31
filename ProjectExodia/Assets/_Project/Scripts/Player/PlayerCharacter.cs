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
        [SerializeField] private Animator orangUtanAnim;
        
        [SerializeField] private float bananaSlapDuration = 10f;

        public PlayerController Controller { get; private set; }
        private HashSet<EntityBase> _entitiesInRange = new();

        private Coroutine _bananaSlapRoutine;
        private bool _isInBananaSlapMode = false;
        
        public bool IsInBananaSlapMode
        {
            get => _isInBananaSlapMode;
            set
            {
                _isInBananaSlapMode = value;
                if (GameContext.Instance.TryGetManager(out UIManager uiManager))
                {
                    uiManager.GetPanel<EffectsPanel>().SetGoBananaFire(value);
                    Debug.Log($"Banana slap mode: {value}");
                }

                if (!value) return;

                IEnumerator SlapRoutine()
                {
                    yield return new WaitForSeconds(bananaSlapDuration);
                    IsInBananaSlapMode = false;
                    uiManager.GetPanel<EffectsPanel>().SetGoBananaFire(false);
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
            Controller = controller;
        }

        public void PerformSlap()
        {
            //Debug.Log($"Slap performed, entities {_entitiesInRange.Count}");
            foreach (var entity in _entitiesInRange)
            {
                Controller.SlapEntity(entity);
            }

            ScoreData.SlapCount += _entitiesInRange.Count;
            orangUtanAnim.Play("Slap");
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

        public Vector3 Position => transform.position;
    }
}
