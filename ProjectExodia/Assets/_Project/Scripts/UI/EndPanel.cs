using TMPro;
using UnityEngine;

namespace ProjectExodia
{
    public class EndPanel : MenuPanel
    {
        [SerializeField] private TextMeshProUGUI slappedCounter;
        [SerializeField] private TextMeshProUGUI distanceCounter;
        [SerializeField] private TextMeshProUGUI countriesCounter;

        public void DisplayStats()
        {
            slappedCounter.text = ScoreData.SlapCount.ToString();
            distanceCounter.text = Mathf.RoundToInt(ScoreData.TotalDistance).ToString();
            countriesCounter.text = ScoreData.CountriesCount.ToString();
        }
    }
}
