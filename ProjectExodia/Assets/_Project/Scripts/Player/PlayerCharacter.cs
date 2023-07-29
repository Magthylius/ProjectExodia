using UnityEngine;

namespace ProjectExodia
{
    public class PlayerCharacter : MonoBehaviour
    {
        public delegate void CharacterEvent();
        public event CharacterEvent OnBananaPickup;
        
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.transform.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.transform.GetComponent<EntityBase>();
            if (!entity) return;
            entity.PerformCollision();
            
            if (entity is BananaEntity)
            {
                OnBananaPickup?.Invoke();
            }
        }

        public void TakeHordeDamage()
        {
            
        }
    }
}
