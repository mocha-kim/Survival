using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    [SerializeField]
    private float realtimePerHour = 30f;
    [SerializeField]
    private float timeLeft;
    private float halftimeOfDay;
    private bool isAM;
    [SerializeField]
    private int day = 1;
    [SerializeField]
    private int hour = 6;
    private float percentHour; // hour / 24, float value between 0 ~ 1

    [SerializeField]
    private int spReduceStandardTime = 20;
    private float spReduceConstValue;
    [SerializeField]
    private int hungerReduceStandardTime = 100;
    private float hungerReduceConstValue;
    [SerializeField]
    private int thirstReduceStandardTime = 100;
    private float thirstReduceConstValue;

    [SerializeField]
    private ClockUI clock;
    private StatsObject playerStats;

    [SerializeField]
    private Light sun;
    private float fadeOutTime;

    public int GetDay() => day;
    public int GetHour() => hour;
    public bool GetIsAM() => isAM;
    public float GetHalftime() => halftimeOfDay;
    public bool GetIsDay() => (isAM && hour >= 5) || (!isAM && hour < 10);

    public Action<int> OnTheHour;
    public Action<WeatherType, float, bool> OnChangeWeather;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        // get time data from file
        GameManager.Instance.OnGameManagerStart += GetTimeData;
    }

    private void Start()
    {
        playerStats = GameManager.Instance.playerStats;

        halftimeOfDay = realtimePerHour * 12;
        spReduceConstValue = 100 / (realtimePerHour * spReduceStandardTime);
        hungerReduceConstValue = 100 / (realtimePerHour * hungerReduceStandardTime);
        thirstReduceConstValue = 100 / (realtimePerHour * thirstReduceStandardTime);

        fadeOutTime = realtimePerHour / 2;
        GetTimeData();
        StartCoroutine(ReduceStaminaByTime());
    }

    private void GetTimeData()
    {
        isAM = GameManager.Instance.gameData.isAM;
        day = GameManager.Instance.gameData.day;
        hour = GameManager.Instance.gameData.hour;
        timeLeft = GameManager.Instance.gameData.timeLeft;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        percentHour = ((isAM ? 12f : 24f) - timeLeft / realtimePerHour) / 24f;
        sun.transform.localRotation = Quaternion.Euler(new Vector3(percentHour * 360f - 90f, 135f, 0f));
        if (12 - (int)Mathf.Ceil(timeLeft / realtimePerHour) != hour)
        {
            hour = 12 - (int)Mathf.Ceil(timeLeft / realtimePerHour);
            if (isAM && hour == 5)
            {
                StartCoroutine(SunFadeIn());
            }
            else if (!isAM && hour == 6)
            {
                StartCoroutine(SunFadeOut());
            }
            OnTheHour?.Invoke(hour);
        }
        if (timeLeft <= 0)
        {
            timeLeft = halftimeOfDay;
            isAM = !isAM;
            day = isAM ? day + 1 : day;
        }
    }

    IEnumerator SunFadeIn()
    {
        sun.gameObject.SetActive(true);
        while (sun.intensity < 1f)
        {
            sun.intensity += Time.deltaTime / fadeOutTime;
            if (sun.intensity >= 1f)
            {
                sun.intensity = 1f;
            }
            yield return null;
        }
    }

    IEnumerator SunFadeOut()
    {
        while (sun.intensity > 0f)
        {
            sun.intensity -= Time.deltaTime / fadeOutTime;
            if (sun.intensity <= 0f)
            {
                sun.intensity = 0f;
            }
            yield return null;
        }
        sun.gameObject.SetActive(false);
    }

    IEnumerator ReduceStaminaByTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            playerStats.AddStatusCurrentValue(StatusType.SP, -spReduceConstValue);
            playerStats.AddStatusCurrentValue(StatusType.Hunger, -hungerReduceConstValue);
            playerStats.AddStatusCurrentValue(StatusType.Thirst, -thirstReduceConstValue);
        }
    }

    public void ResetTime()
    {
        timeLeft = GameManager.Instance.gameData.timeLeft = halftimeOfDay / 2;
        isAM = GameManager.Instance.gameData.isAM = true;
        day = GameManager.Instance.gameData.day = 1;
        hour = GameManager.Instance.gameData.hour = 6;
    }
}
