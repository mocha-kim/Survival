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
    private int day;
    [SerializeField]
    private int hour;

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

    public int GetDay() => day;
    public int GetHour() => hour;
    public bool GetIsAM() => isAM;
    public float GetHalftime() => halftimeOfDay;

    public Action<int> OnTheHour;

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
    }

    private void Start()
    {
        playerStats = GameManager.Instance.playerStats;

        halftimeOfDay = realtimePerHour * 12;
        spReduceConstValue = 100 / (realtimePerHour * spReduceStandardTime);
        hungerReduceConstValue = 100 / (realtimePerHour * hungerReduceStandardTime);
        thirstReduceConstValue = 100 / (realtimePerHour * thirstReduceStandardTime);

        // get time data from file
        timeLeft = GameManager.Instance.gameData.timeLeft;
        isAM = GameManager.Instance.gameData.isAM;
        day = GameManager.Instance.gameData.day;

        StartCoroutine(ReduceStaminaByTime());
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (12 - (int)Mathf.Ceil(timeLeft / realtimePerHour) != hour)
        {
            hour = 12 - (int)Mathf.Ceil(timeLeft / realtimePerHour);
            OnTheHour?.Invoke(hour);
        }
        if (timeLeft <= 0)
        {
            timeLeft = halftimeOfDay;
            isAM = !isAM;
            day = isAM ? day + 1 : day;
            clock.UpdateClockUI();  
        }
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
        timeLeft = GameManager.Instance.gameData.timeLeft = halftimeOfDay;
        isAM = GameManager.Instance.gameData.isAM = true;
        day = GameManager.Instance.gameData.day = 1;
    }
}
