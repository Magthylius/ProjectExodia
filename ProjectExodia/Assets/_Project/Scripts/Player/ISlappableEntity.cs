using UnityEngine;

namespace ProjectExodia
{
    public interface ISlappableEntity
    {
        public bool PerformSlap();
        public void PerformCollision();
        public bool GetWasSlapped();
        public string GetName();
    }
}