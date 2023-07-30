using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectExodia
{
    [Serializable]
    public struct FLevelData
    {
        public int level;
        public int requiredExperience;
    }
    
    public class StampedeManager : ManagerBase
    {
        [Header("Stampede Parameters")]
        [SerializeField] private int experienceDecay = 2;
        [SerializeField] private FLevelData[] levelData;

        public static event Action OnSetLevel;
        public static event Action OnAddExperience;
        public static event Action OnSubtractExperience;
        
        private int _stampedeLevel;
        private int _stampedeExperience;

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            SetLevel(1);
        }

        private void Start()
        {
            SetLevel(1);
        }

        private void SetLevel(int amount)
        {
            _stampedeLevel = amount;
            OnSetLevel?.Invoke();
        }

        public void AddExperience()
        {
            if (GameManager.GameState != GameState.Gameplay) return;
            
            if (_stampedeLevel >= levelData.Length)
            {
                Debug.Log("Reached max level");
                if (GameContext.TryGetManager(out UIManager uiManager))
                {
                    uiManager.GetPanel<EffectsPanel>().SetLoseStampede(true);
                    StartCoroutine(StampedeRoutine());
                    
                    IEnumerator StampedeRoutine()
                    {
                        //! Arbitrary number
                        yield return new WaitForSeconds(3f);
                        var transition = uiManager.ShowPanel<TransitionPanel>(false);
                        transition.BeginTransition();
                        transition.OnFullTransition += OnTransition;
                        
                        void OnTransition()
                        {
                            transition.OnFullTransition -= OnTransition;
                            uiManager.ShowPanel<EndPanel>(false).DisplayStats();
                        }
                    }
                }

                GameManager.GameState = GameState.End;
                if (GameContext.TryGetManager(out GameManager gameManager))
                {
                    gameManager.StopGameplay();
                }
                return;
            }
            _stampedeExperience++;
            
            if (_stampedeExperience >= levelData[_stampedeLevel - 1].requiredExperience)
            {
                SetLevel(_stampedeLevel + 1);
                if (GameContext.TryGetManager(out UIManager uiManager))
                {
                    uiManager.GetPanel<EffectsPanel>().LevelUpStampede();
                }

                _stampedeExperience = 0;
            }

            OnAddExperience?.Invoke();
        }

        public void SubtractExperience()
        {
            _stampedeExperience -= experienceDecay;
            if (_stampedeLevel <= 1)
            {
                _stampedeExperience = 0;
                Debug.Log("Reached min level");
                return;
            }

            if (_stampedeExperience < 0)
            {
                SetLevel(_stampedeLevel - 1);
                if (GameContext.TryGetManager(out UIManager uiManager))
                {
                    uiManager.GetPanel<EffectsPanel>().LevelDownStampede();
                }

                _stampedeExperience =  levelData[_stampedeLevel - 1].requiredExperience + _stampedeExperience;
            }
            
            OnSubtractExperience?.Invoke();
        }
        
    }
}