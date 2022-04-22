using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeatherInfo
{
    public int level;
    public int[] day;
    public int[] rain;
    public int[] fog;
}

[Serializable]
public class WeatherInfoContainer
{
    public WeatherInfo[] info;
}