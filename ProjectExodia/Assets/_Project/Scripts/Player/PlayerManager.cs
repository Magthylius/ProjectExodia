using UnityEngine;

namespace ProjectExodia
{
    public class PlayerManager : ManagerBase
    {
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        
        [Header("Settings")]
        [SerializeField] private AudioData slapAudioData;
        [SerializeField] private int goldenBananaCost = 3;

        private int _goldenBananas = 0;
        
        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            playerController.OnEntitySlapped += OnEntitySlapped;
            playerController.OnPlayerTriggered += OnPlayerTriggered;

            if (gameContext.TryGetManager(out CameraManager cameraManager))
            {
                playerController.Initialize(cameraManager);
                playerController.Character.OnBananaPickup += GainBanana;
            }
        }

        private void OnEntitySlapped(ISlappableEntity entity)
        {
            if (GameContext.TryGetManager<AudioManager>(out var audioManager))
            {
                audioManager.PlaySfx(slapAudioData, "Slap");
            }
            
            if (entity is BananaEntity && !Controller.Character.IsInBananaSlapMode)
            {
                GainBanana();
            }
            else if (entity is EnemyEntity)
            {
                ScoreData.AddScore(ScoreData.ENEMY_BASE_SCORE);
                ScoreData.IncreaseMultiplier(ScoreData.MULTIPLIER_BASE_INCREMENT);
            }
        }

        private void OnPlayerTriggered(PlayerCharacter playerCharacter)
        {
            if (_goldenBananas < goldenBananaCost) return;
            _goldenBananas -= goldenBananaCost;
            ActivateAbility();
        }

        private void GainBanana()
        {
            if (_goldenBananas < goldenBananaCost)
            {
                _goldenBananas++;
                Debug.Log(_goldenBananas);
            }
        }
        
        private void ActivateAbility()
        {
            Controller.Character.IsInBananaSlapMode = true;
        }

        public PlayerController Controller => playerController;
    }
}