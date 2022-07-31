using System;
using System.IO;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private WeatherType weather;
    private WeatherType beforeWeather;
    [SerializeField]
    private float temperature;

    private WeatherInfo[] info;
    private bool isAfterRain = false;
    private int afterRainWeight = 15;

    [SerializeField]
    private GameObject rainEffects;
    [SerializeField]
    private GameObject snowEffects;
    [SerializeField]
    private GameObject fogEffects;

    private GameObject activeEffects;
    private int activationHour;

    private int Day => TimeManager.Instance.GetDay();
    private bool IsAM => TimeManager.Instance.GetIsAM();

    private void Start()
    {
        weather = GameManager.Instance.gameData.weather;
        activationHour = GameManager.Instance.gameData.weatherActivationHour;
        temperature = GameManager.Instance.gameData.temperature;

        LoadWeatherJson();
        ChangeWeather(weather);

        TimeManager.Instance.OnTheHour += OnTheHour;
    }

    private void LoadWeatherJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/GameSystems/EnvironmentSystem/weather.json");
        WeatherInfoContainer data = JsonUtility.FromJson<WeatherInfoContainer>(json);

        info = data.info;
    }

    public void OnTheHour(int hour)
    {
        activationHour--;

        beforeWeather = weather;
        if (IsAM && hour == 5)
        {
            SetNextRandomWeather(true);
        }
        else if (activationHour <= 0)
        {
            ChangeWeather(WeatherType.Sunny);
            SetNextRandomWeather(false);
        }

        temperature += ModifyTemperature(hour);
        temperature = ClampTemperature(hour);

        TimeManager.Instance.OnChangeWeather?.Invoke(weather, temperature, weather == beforeWeather ? true : false);
    }

    private void ChangeWeather(WeatherType weather)
    {
        if (activeEffects)
        {
            activeEffects.SetActive(false);
            activeEffects = null;
        }

        switch (weather)
        {
            case WeatherType.Rainy:
                rainEffects.SetActive(true);
                activeEffects = rainEffects;
                break;
            case WeatherType.Snowy:
                snowEffects.SetActive(true);
                activeEffects = snowEffects;
                break;
            case WeatherType.Foggy:
                fogEffects.SetActive(true);
                activeEffects = fogEffects;
                break;
        }
        this.weather = weather;
    }

    private void SetNextRandomWeather(bool fogTime)
    {
        int level = 0;
        switch (Day)
        {
            case int i when (i > 30):
                level = 4;
                break;
            case int i when (20 < i && i <= 30):
                level = 3;
                break;
            case int i when (10 < i && i <= 20):
                level = 2;
                break;
            case int i when (2 < i && i <= 10):
                level = 1;
                break;
            case int i when (i <= 2):
                level = 0;
                break;
        }
        Debug.Log("Weather Difficulty Level: " + level);

        int r = UnityEngine.Random.Range(0, 100);
        if (fogTime)
        {
            int fog = info[level].fog[1] + (isAfterRain ? afterRainWeight : 0);
            if (r < fog)
            {
                ChangeWeather(WeatherType.Foggy);
                activationHour = 19;
                Debug.Log("Weather Changed to Foggy, during " + activationHour + "hours");
                return;
            }
            isAfterRain = false;
        }

        int rain = info[level].rain[1];
        if (r < rain)
        {
            WeatherType type =  temperature < -4 ? WeatherType.Snowy : WeatherType.Rainy;
            ChangeWeather(type);
            activationHour = UnityEngine.Random.Range(2, 6);
            isAfterRain = true;
            Debug.Log("Weather Changed to " + type.ToString() + ", during " + activationHour + "hours");
        }
        else
        {
            ChangeWeather(WeatherType.Sunny);
            activationHour = UnityEngine.Random.Range(4, 8);
            Debug.Log("Weather Changed to Sunny, during " + activationHour + "hours");
        }
    }

    private float ModifyTemperature(int hour)
    {
        if (IsAM)
        {
            if (hour < 5)                       // 0 ~ 5
            {
                return -UnityEngine.Random.Range(0.5f, 2f);
            }
            else if (5 <= hour && hour < 10)    // 5 ~ 10
            {
                return UnityEngine.Random.Range(0.5f, 1.5f);
            }
            else                                // 10 ~ 12
            {
                return UnityEngine.Random.Range(0.5f, 2.5f);
            }
        }
        else
        {
            if (hour < 3)                       // 12 ~ 15
            {
                return UnityEngine.Random.Range(0.5f, 1.5f);
            }
            else if (3 <= hour && hour < 8)     // 15 ~ 20
            {
                return -UnityEngine.Random.Range(0.5f, 1f);
            }
            else                                // 20 ~ 24
            {
                return -UnityEngine.Random.Range(0.5f, 2.5f);
            }
        }
    }

    private float ClampTemperature(int hour)
    {
        if (IsAM)
        {
            if (hour < 5)                       // 0 ~ 5
            {
                return Mathf.Clamp(temperature, -20f, -5f);
            }
            else if (5 <= hour && hour < 10)    // 5 ~ 10
            {
                return Mathf.Clamp(temperature, -5f, 5f);
            }
            else                                // 10 ~ 12
            {
                return  Mathf.Clamp(temperature, 5f, 15f);
            }
        }
        else
        {
            if (hour < 4)                       // 12 ~ 16
            {
                return Mathf.Clamp(temperature, 5f, 15f);
            }
            else if (4 <= hour && hour < 8)     // 16 ~ 20
            {
                return Mathf.Clamp(temperature, -5f, 5f);
            }
            else                                // 20 ~ 24
            {
                return Mathf.Clamp(temperature, -20f, -5f);
            }
        }
    }
}
