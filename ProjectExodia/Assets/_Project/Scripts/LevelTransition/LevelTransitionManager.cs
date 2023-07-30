using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public enum ECountry
    {
        NONE = 0,
        MALAYSIA,
        JAPAN,
        INDIA,
        SOUTHPOLE
    }
    
    public class LevelTransitionManager : ManagerBase
    {
        [SerializeField] private CountryPack Malaysia;
        [SerializeField] private CountryPack Japan;
        [SerializeField] private CountryPack India;
        [SerializeField] private CountryPack Southpole;
        
        private TileHandler _tileHandler;
        private int _lastCountryVisited;

        public static ECountry CurrentCountry;
        public static event Action<CountryPack> OnCountryChange;
        
        
        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
        }

        private void Start()
        {
            ChangeCountry();
            InitiateLevelTransit();
        }

        public void ChangeCountry()
        {
            CurrentCountry = (ECountry)RandomCountry();
        }

        public void InitiateLevelTransit()
        {
            switch (CurrentCountry)
            {
                case ECountry.MALAYSIA:
                    OnCountryChange?.Invoke(Malaysia);
                    break;
                case ECountry.JAPAN:
                    OnCountryChange?.Invoke(Japan);
                    break;
                case ECountry.INDIA:
                    OnCountryChange?.Invoke(India);
                    break;
                case ECountry.SOUTHPOLE:
                    OnCountryChange?.Invoke(Southpole);
                    break;
            }
        }
        
        private int RandomCountry()
        {
            var randomIndex = _lastCountryVisited;

            while (randomIndex == _lastCountryVisited)
                randomIndex = Random.Range(1, 5);
            
            _lastCountryVisited = randomIndex;
            return randomIndex;
        }
    }
}