using System;
using System.Collections;
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
        SOUTHPOLE,
        NORTHPOLE
    }
    
    public class LevelTransitionManager : ManagerBase
    {
        [SerializeField, Min(1f)] private float maxDistance = 2000f;
        
        [SerializeField] private CountryPack Malaysia;
        [SerializeField] private CountryPack Japan;
        [SerializeField] private CountryPack India;
        [SerializeField] private CountryPack Southpole;
        [SerializeField] private CountryPack Northpole;

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

            Debug.Log(GameManager.GameState);
            if (_playerCharacter.Position.z >= maxDistance && GameManager.GameState == GameState.Gameplay)
            {
                ChangeCountry(true);
                _playerCharacter.Controller.TeleportCharacter(Vector3.zero);
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

        public void ChangeCountry(bool wantsTransition = false)
        {
            if (wantsTransition)
            {
                StartCoroutine(ChangeRoutine());
                ScoreData.TotalDistance += maxDistance;
            }
            else InitiateLevelTransit();

            IEnumerator ChangeRoutine()
            {
                if (GameContext.TryGetManager(out UIManager uiManager))
                {
                    uiManager.ShowPanel<TransitionPanel>(false).BeginTransition();
                }

                if (GameContext.TryGetManager(out GameManager gameManager))
                {
                    gameManager.StopGameplay();
                }
                
                yield return new WaitForSeconds(1.5f);
                InitiateLevelTransit();
                
                yield return new WaitForSeconds(3f);
                gameManager.StartGameplay();
                GameManager.GameState = GameState.Gameplay;
            }
        }

        public void InitiateLevelTransit()
        {
            Debug.Log("Country has changed");
            CurrentCountry = (ECountry)RandomCountry();
            _playerCharacter.Controller.RampMovementSpeed();
            ScoreData.CountriesCount++;
            
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
                case ECountry.NORTHPOLE:
                    OnCountryChange?.Invoke(Northpole);
                    break;
            }
        }
        
        private int RandomCountry()
        {
            var randomIndex = _lastCountryVisited;

            while (randomIndex == _lastCountryVisited)
                randomIndex = Random.Range(1, 6);
            
            _lastCountryVisited = randomIndex;
            return randomIndex;
        }
    }
}