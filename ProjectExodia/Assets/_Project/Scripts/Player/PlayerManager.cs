using UnityEngine;

namespace ProjectExodia
{
    public class PlayerManager : ManagerBase
    {
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        
        [Header("Settings")]
        [SerializeField] private AudioData slapAudioData;
        
        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            playerController.OnEntitySlapped += OnEntitySlapped;
        }

        private void OnEntitySlapped(ISlappableEntity entity)
        {
            if (GameContext.TryGetManager<AudioManager>(out var audioManager))
            {
                audioManager.PlaySfx(slapAudioData, "Slap");
            }
        }

        public PlayerController Controller => playerController;
    }
}