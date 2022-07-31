using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    [SerializeField]
    private Material sunrise;
    [SerializeField]
    private Material day;
    [SerializeField]
    private Material sunset;
    [SerializeField]
    private Material night;
    [SerializeField]
    private Material rainey;

    [SerializeField]
    private Color fog;
    [SerializeField]
    private Color nightFog;
    [SerializeField]
    private Color sunsetFog;

    private bool isDay;
    private bool IsAM => TimeManager.Instance.GetIsAM();

    private void Start()
    {
        TimeManager.Instance.OnTheHour += OnTheHour;
        TimeManager.Instance.OnChangeWeather += OnChangeWeather;
        isDay = TimeManager.Instance.GetIsDay();
        ChangeSky(GameManager.Instance.gameData.sky);
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.5f);
    }

    public void OnTheHour(int hour)
    {
        if (IsAM)
        {
            switch(hour)
            {
                case 5:
                    isDay = true;
                    ChangeSky(SkyType.Sunrise);
                    break;
                case 8:
                    ChangeSky(SkyType.Day);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch(hour)
            {
                case 7:
                    ChangeSky(SkyType.Sunset);
                    break;
                case 10:
                    isDay = false;
                    ChangeSky(SkyType.Night);
                    break;
                default:
                    break;
            }
        }
    }

    public void OnChangeWeather(WeatherType weather, float temperature, bool is_changed)
    {
        switch(weather)
        {
            case WeatherType.Rainy:
            case WeatherType.Foggy:
                if (isDay)
                    ChangeSky(SkyType.Rainey);
                break;
            default:
                break;
        }
    }

    private void ChangeSky(SkyType sky)
    {
        switch (sky)
        {
            case SkyType.Sunrise:
                RenderSettings.skybox = sunrise;
                RenderSettings.fog = true;
                RenderSettings.fogColor = fog;
                break;
            case SkyType.Day:
                RenderSettings.skybox = day;
                RenderSettings.fog = false;
                break;
            case SkyType.Sunset:
                RenderSettings.skybox = sunset;
                RenderSettings.fog = true;
                RenderSettings.fogColor = sunsetFog;
                break;
            case SkyType.Night:
                RenderSettings.skybox = night;
                RenderSettings.fog = true;
                RenderSettings.fogColor = nightFog;
                break;
            case SkyType.Rainey:
                RenderSettings.skybox = rainey;
                RenderSettings.fog = true;
                RenderSettings.fogColor = fog;
                break;
            default:
                RenderSettings.skybox = day;
                RenderSettings.fog = false;
                break;
        }
    }
}
