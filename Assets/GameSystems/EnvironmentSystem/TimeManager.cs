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
    private bool isDaytime;
    private int day;

    [SerializeField]
    private int spReduceStandardTime = 20;
    private float spReduceConstValue;
    [SerializeField]
    private int hungerReduceStandardTime = 100;
    private float hungerReduceConstValue;
    [SerializeField]
    private int thirstReduceStandardTime = 100;
    private float thirstReduceConstValue;

    private ClockUI clock;
    private StatsObject playerStats;

    public float GetRealtimePerHour() => realtimePerHour;
    public float GetTimeLeft() => timeLeft;
    public bool GetIsDaytime() => isDaytime;
    public int GetDay() => day;

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
        timeLeft = halftimeOfDay;
        isDaytime = true;
        day = 1;

        StartCoroutine(ReduceStaminaByTime());
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = halftimeOfDay;
            isDaytime = !isDaytime;
            day = isDaytime ? day + 1 : day;
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
}
