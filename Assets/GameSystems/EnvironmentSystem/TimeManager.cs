using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    public float realtimePerHour = 60f;
    [NonSerialized]
    public float halftimeOfDay;
    public float timeLeft;
    public bool isDaytime;

    public ClockUI clock;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        halftimeOfDay = realtimePerHour * 12;

        // get time data from file
        timeLeft = halftimeOfDay * 2;
        isDaytime = true;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= halftimeOfDay && isDaytime)
        {
            isDaytime = !isDaytime;
            clock.ChangeBackgroundSprite(isDaytime);
        }
        else if (timeLeft <= 0 && !isDaytime)
        {
            timeLeft = halftimeOfDay * 2;
            isDaytime = !isDaytime;
            clock.ChangeBackgroundSprite(isDaytime);
        }
    }
}
