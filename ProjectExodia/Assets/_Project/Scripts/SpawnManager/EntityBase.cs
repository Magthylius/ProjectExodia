using System.Collections;
using System.Collections.Generic;
using ProjectExodia;
using UnityEngine;

namespace ProjectExodia
{
    public abstract class EntityBase : MonoBehaviour, ISlappableEntity
    {
        [Header("Entity Base")]
        [SerializeField] private bool allowMultipleSlaps = false;

        protected GameContext GameContext;
        protected bool WasSlapped;

        public virtual void Initialize(GameContext gameContext)
        {
            GameContext = gameContext;
        }
        
        public virtual bool PerformSlap()
        {
            if (!allowMultipleSlaps && WasSlapped) return false;
            WasSlapped = true;
            return true;
        }

        public virtual bool GetWasSlapped() => WasSlapped;
        public virtual string GetName() => gameObject.name;
    }
}
