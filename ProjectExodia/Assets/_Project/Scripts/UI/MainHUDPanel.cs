using System.Globalization;
using TMPro;
using UnityEngine;

namespace ProjectExodia
{
    public class MainHUDPanel : MenuPanel
    {
        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private TextMeshProUGUI multiplierTMP;

        private void Awake()
        {
            ScoreData.OnScoreIncreased += score => scoreTMP.text = score.ToString();
            ScoreData.OnMultiplierIncreased += multiplier => multiplierTMP.text = "x" + multiplier.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
