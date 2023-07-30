using System;
using UnityEngine;

namespace ProjectExodia
{
    public struct ScoreData
    {
        public const int ENEMY_BASE_SCORE = 100;
        public const float MULTIPLIER_BASE_INCREMENT = 0.25f;
        
        public delegate void ScoreEvent(int score);
        public delegate void MultiplierEvent(float multiplier);
        public static event ScoreEvent OnScoreIncreased;
        public static event MultiplierEvent OnMultiplierIncreased;
        
        public static int CurrentScore { get; private set; }
        public static float CurrentMultiplier { get; private set; }

        public static int SlapCount;
        public static float TotalDistance;
        public static int CountriesCount;

        public static void Reset()
        {
            CurrentScore = 0;
            CurrentMultiplier = 1f;

            SlapCount = 0;
            TotalDistance = 0f;
            CountriesCount = 0;
        }

        public static void AddScore(int scoreToAdd)
        {
            CurrentScore += (int)(scoreToAdd * CurrentMultiplier);
            OnScoreIncreased?.Invoke(CurrentScore);
            
            //Debug.Log($"Current score {CurrentScore}");
        }

        public static void IncreaseMultiplier(float multiplierIncrement)
        {
            CurrentMultiplier += multiplierIncrement;
            OnMultiplierIncreased?.Invoke(CurrentMultiplier);
            //Debug.Log($"Current multiplier {CurrentMultiplier}");
        }
    }

    public enum GameState
    {
        MainMenu,
        Gameplay,
        Transition
    }
    
    public class GameManager : ManagerBase
    {
        private void Awake()
        {
            ScoreData.Reset();
        }

        private void Start()
        {
            StopGameplay();
        }

        public void StartGameplay()
        {
            if (GameContext.TryGetManager(out TileManager tileManager)) tileManager.SetSpawnTile(true);
            if (GameContext.TryGetManager(out SpawnManager spawnManager)) spawnManager.SetSpawnEntities(true);
            if (GameContext.TryGetManager(out PlayerManager playerManager)) playerManager.Controller.SetAllowMovement(true);
        }

        public void StopGameplay()
        {
            if (GameContext.TryGetManager(out TileManager tileManager)) tileManager.SetSpawnTile(false);
            if (GameContext.TryGetManager(out SpawnManager spawnManager)) spawnManager.SetSpawnEntities(false);
            if (GameContext.TryGetManager(out PlayerManager playerManager)) playerManager.Controller.SetAllowMovement(false);
        }
    }
}