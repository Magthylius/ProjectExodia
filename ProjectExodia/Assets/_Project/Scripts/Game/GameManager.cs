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

        public static void Reset()
        {
            CurrentScore = 0;
            CurrentMultiplier = 1f;
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
    
    public class GameManager : ManagerBase
    {
        private void Awake()
        {
            ScoreData.Reset();
        }
    }
}