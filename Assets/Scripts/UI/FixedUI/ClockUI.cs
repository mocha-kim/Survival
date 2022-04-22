using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockUI : MonoBehaviour
{
    private Slider clock;

    [SerializeField]
    private Image fill;
    [SerializeField]
    private Color[] fillColors;

    [SerializeField]
    private TextMeshProUGUI dayText;
    [SerializeField]
    private Color[] textColors;

    private int maxValue = 12;
    private int calcValue;

    private bool IsAM => TimeManager.Instance.GetIsAM();

    private void Start()
    {
        // init image components
        fill.color = IsAM ? fillColors[0] : fillColors[1];
        dayText.color = IsAM ? textColors[0] : textColors[1];

        // init clock vlaues
        clock = GetComponent<Slider>();
        calcValue = maxValue;
        clock.maxValue = maxValue;
        clock.value = maxValue;

        TimeManager.Instance.OnTheHour += OnTheHour;
    }

    public void OnTheHour(int hour)
    {
        clock.value = 12 - hour;
    }

    public void UpdateClockUI()
    {
        fill.color = IsAM ? fillColors[0] : fillColors[1];
        dayText.color = IsAM ? textColors[0] : textColors[1];
        dayText.text = "DAY " + TimeManager.Instance.GetDay();
        calcValue = maxValue;
        clock.value = maxValue;
    }
}
