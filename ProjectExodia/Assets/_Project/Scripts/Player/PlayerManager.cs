using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class PlayerManager : ManagerBase
    {
        private PlayerController _controller;
        
        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            
            _controller = gameObject.AddComponent<PlayerController>();
            _controller.Initialize(gameContext.GetGameData());
        }
    }
}