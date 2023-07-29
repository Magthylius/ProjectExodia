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
            
            if (gameContext.TryGetManager(out CameraManager cameraManager))
                playerController.Initialize(cameraManager);
        }

        private void OnEntitySlapped(ISlappableEntity entity)
        {
            if (GameContext.TryGetManager<AudioManager>(out var audioManager))
            {
                audioManager.PlaySfx(slapAudioData, "Slap");
            }
            
            if (entity is BananaEntity && _goldenBananas < goldenBananaCost)
            {
                _goldenBananas++;
                Debug.Log(_goldenBananas);
            }
        }

        private void ActivateAbility()
        {
            
        }
        
        public void UseGoldenBananas()
        {
            if (_goldenBananas < goldenBananaCost) return;
            _goldenBananas -= goldenBananaCost;
            ActivateAbility();
        }

        public PlayerController Controller => playerController;
    }
}