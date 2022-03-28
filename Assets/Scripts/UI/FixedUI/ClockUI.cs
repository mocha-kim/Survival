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

    private float TimeLeft => TimeManager.Instance.timeLeft;
    private bool IsDaytime => TimeManager.Instance.isDaytime;

    private void Start()
    {
        // init image components
        fill.color = IsDaytime ? fillColors[0] : fillColors[1];
        dayText.color = IsDaytime ? textColors[0] : textColors[1];

        // init clock vlaues
        clock = GetComponent<Slider>();
        calcValue = maxValue;
        clock.maxValue = maxValue;
        clock.value = maxValue;
    }

    private void Update()
    {
        calcValue = (int)Mathf.Ceil(TimeLeft / (float)TimeManager.Instance.realtimePerHour);
        if (clock.value > calcValue)
        {
            clock.value = calcValue;
        }
    }

    public void UpdateClockUI()
    {
        fill.color = IsDaytime ? fillColors[0] : fillColors[1];
        dayText.color = IsDaytime ? textColors[0] : textColors[1];
        dayText.text = "DAY " + TimeManager.Instance.day;
        calcValue = maxValue;
        clock.value = maxValue;
    }
}
