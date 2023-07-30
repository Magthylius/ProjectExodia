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
        [SerializeField, Min(1f)] private float maxDistance = 2000f;
        
        [SerializeField] private CountryPack Malaysia;
        [SerializeField] private CountryPack Japan;
        [SerializeField] private CountryPack India;
        [SerializeField] private CountryPack Southpole;

        private PlayerCharacter _playerCharacter;
        private TileHandler _tileHandler;
        private int _lastCountryVisited;

        public static ECountry CurrentCountry;
        public static event Action<CountryPack> OnCountryChange;
        
        private void Start()
        {
            ChangeCountry();
        }

        private void LateUpdate()
        {
            if (!_playerCharacter) return;

            if (_playerCharacter.Position.z >= maxDistance)
            {
                ChangeCountry();
                _playerCharacter.Teleport(Vector3.zero);
            }
        }

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            if (gameContext.TryGetManager(out PlayerManager playerManager))
            {
                _playerCharacter = playerManager.Controller.Character;
            }
        }

        public void ChangeCountry()
        {
            Debug.Log("Country has changed");
            CurrentCountry = (ECountry)RandomCountry();
            InitiateLevelTransit();
            _playerCharacter.Controller.RampMovementSpeed();
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