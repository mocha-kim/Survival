using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeatherUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI weatherText;
    [SerializeField]
    private TextMeshProUGUI temperatureText;

    void Start()
    {
        TimeManager.Instance.OnChangeWeather += OnChangeWeather;
        OnChangeWeather(GameManager.Instance.gameData.weather, GameManager.Instance.gameData.temperature);
    }

    public void OnChangeWeather(WeatherType weather, float temperature, bool is_changed = true)
    {
        weatherText.text = weather.ToString();
        temperatureText.text = temperature.ToString("f1") + "°C";
    }
}
