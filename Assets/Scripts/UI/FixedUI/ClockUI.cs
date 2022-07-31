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
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Color[] textColors;

    private int maxValue = 12;

    private bool IsAM => TimeManager.Instance.GetIsAM();

    private void Awake()
    {
        // init clock vlaues
        clock = GetComponent<Slider>();
        clock.maxValue = maxValue;
        clock.value = maxValue;

        GameManager.Instance.OnGameManagerStart += ClockUIInit;
        TimeManager.Instance.OnTheHour += OnTheHour;
    }

    private void ClockUIInit()
    {
        OnTheHour(GameManager.Instance.gameData.hour);
    }

    public void OnTheHour(int hour)
    {
        Debug.Log("Now " + (hour == 0 ? 12 : hour) + (IsAM ? " AM" : " PM"));
        clock.value = 12 - hour;
        timeText.text = (hour == 0 ? 12 : hour) + (IsAM ? " AM" : " PM");
        if (hour == 6)
            UpdateClockUI(hour);
        else if (hour == 0)
            dayText.text = "DAY " + TimeManager.Instance.GetDay();
    }

    public void UpdateClockUI(int hour)
    {
        bool is_darkmode = IsAM && (hour < 6) || !IsAM && (hour >= 6);
        fill.color = is_darkmode ? fillColors[1] : fillColors[0];
        dayText.color = is_darkmode ? textColors[1] : textColors[0];
        timeText.color = is_darkmode ? Color.white : Color.black;
    }
}
