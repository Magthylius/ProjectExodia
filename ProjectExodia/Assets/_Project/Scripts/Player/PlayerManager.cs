using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace ProjectExodia
{
    public class PlayerManager : ManagerBase
    {
        [SerializeField] private PlayerController playerController;
        
        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
        }
    }
}