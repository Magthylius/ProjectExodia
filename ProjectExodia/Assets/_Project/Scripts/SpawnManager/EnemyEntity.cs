using MoreMountains.Feedbacks;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : MonoBehaviour, ISlappableEntity
    {
        [SerializeField] private MMF_Player effectsPlayer;
        
        public void PerformSlap()
        {
            effectsPlayer.PlayFeedbacks();
        }

        public string GetName() => gameObject.name;
    }
}
