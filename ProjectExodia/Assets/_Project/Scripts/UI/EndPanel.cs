using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExodia
{
    public class EndPanel : MenuPanel
    {
        [SerializeField] private TextMeshProUGUI slappedCounter;
        [SerializeField] private TextMeshProUGUI distanceCounter;
        [SerializeField] private TextMeshProUGUI countriesCounter;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButton);
        }

        public void DisplayStats()
        {
            slappedCounter.text = ScoreData.SlapCount.ToString();
            distanceCounter.text = Mathf.RoundToInt(ScoreData.TotalDistance).ToString();
            countriesCounter.text = ScoreData.CountriesCount.ToString();
        }

        private void OnRestartButton()
        {
            if (GameContext.Instance.TryGetManager(out GameManager gameManager))
            {
                gameManager.RestartGame();
            }
        }
    }
}
