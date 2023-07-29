using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : EntityBase
    {
        [SerializeField] private MMF_Player effectsPlayer;
        [SerializeField] private float despawnOffset = 10.0f;
        
        private void Update()
        {
            if (PlayerTransform && PlayerTransform.position.z - despawnOffset > transform.position.z)
            {
                Destroy(gameObject);
            }
        }
        
        public override bool PerformSlap()
        {
            if (!base.PerformSlap()) return false;

            effectsPlayer.PlayFeedbacks();
            return true;
        }
    }
}
