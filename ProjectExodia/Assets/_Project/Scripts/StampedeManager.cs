using System;
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

        private static readonly int LevelUp = Animator.StringToHash("LevelUp");
        private static readonly int LevelDown = Animator.StringToHash("LevelDown");

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            Setlevel(1);
        }

        private void Start()
        {
            Setlevel(1);
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AddExperience();
            }
            
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                SubtractExperience();
            }
        }

        private void Setlevel(int amount)
        {
            _stampedeLevel = amount;
            OnSetLevel?.Invoke();
        }

        public void AddExperience()
        {
            if (_stampedeLevel >= levelData.Length)
            {
                Debug.Log("Reached max level");
                return;
            }
            _stampedeExperience++;
            
            if (_stampedeExperience >= levelData[_stampedeLevel - 1].requiredExperience)
            {
                Setlevel(_stampedeLevel + 1);
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
                Setlevel(_stampedeLevel - 1);
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