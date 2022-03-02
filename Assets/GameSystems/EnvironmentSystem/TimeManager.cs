using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    public float realtimePerHour = 30f;
    public float timeLeft;
    [NonSerialized]
    public float halftimeOfDay;
    [NonSerialized]
    public bool isDaytime;
    [NonSerialized]
    public int day;

    public ClockUI clock;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        halftimeOfDay = realtimePerHour * 12;

        // get time data from file
        timeLeft = halftimeOfDay;
        isDaytime = true;
        day = 1;
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
}
