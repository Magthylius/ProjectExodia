using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public abstract class ManagerBase : MonoBehaviour
    {
        protected GameContext GameContext;

        public virtual void Initialize(GameContext gameContext)
        {
            GameContext = gameContext;
        }
    }
}
