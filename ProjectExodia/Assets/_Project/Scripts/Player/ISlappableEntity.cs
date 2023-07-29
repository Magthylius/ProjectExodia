using UnityEngine;

namespace ProjectExodia
{
    public interface ISlappableEntity
    {
        public bool PerformSlap();
        public bool GetWasSlapped();
        public string GetName();
    }
}