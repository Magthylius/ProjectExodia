using System;
using System.Collections;
using System.Collections.Generic;
using ProjectExodia;
using UnityEngine;

namespace ProjectExodia
{
    public class LevelTransitionManager : ManagerBase
    {
        [SerializeField] private CountryPack[] countryPacks;
        private TileHandler _tileHandler;

        public override void Initialize(GameContext gameContext)
        {
            base.Initialize(gameContext);
            
            gameContext.TryGetManager(out TileManager tileManager);
            _tileHandler = tileManager.TileHandler;
        }

        private void Start()
        {
            InitiateLevelTransit();
        }

        public void InitiateLevelTransit()
        {
            _tileHandler.UpdateTile(countryPacks[0].FloorTexture);
        }
    }
}