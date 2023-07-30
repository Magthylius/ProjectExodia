using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : EntityBase
    {
        [SerializeField] private MMF_Player effectsPlayer;
        [SerializeField] private float despawnOffset = 10.0f;
        [SerializeField] private Animator anim;
        
        private PlayerCharacter _playerCharacter;
        private StampedeManager _stampedeManager;
        private static readonly int Slap = Animator.StringToHash("Slap");


        private void Update()
        {
            if (_playerCharacter && _playerCharacter.transform.position.z - despawnOffset > transform.position.z)
            {
                if (!WasSlapped)
                    _stampedeManager.AddExperience();
                
                Destroy(gameObject);
            }
        }

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            if (gameContext.TryGetManager(out PlayerManager manager))
                _playerCharacter = manager.Controller.Character;
            
            if (gameContext.TryGetManager(out StampedeManager stampedeManager))
                _stampedeManager = stampedeManager;
        }

        public override bool PerformSlap()
        {
            if (!base.PerformSlap()) return false;
            effectsPlayer.PlayFeedbacks();
            anim.SetTrigger(Slap);
            return true;
        }

        public override void PerformCollision()
        {
        }
    }
}
