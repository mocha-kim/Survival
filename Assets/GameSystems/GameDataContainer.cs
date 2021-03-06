using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Play Data")]
public class GameDataContainer : ScriptableObject
{
    public string playerName;
    public int selectedQuickslot;

    public float timeLeft;
    public bool isAM;
    public int day;
    public int hour;

    public WeatherType weather;
    public SkyType sky;
    public int weatherActivationHour;
    public float temperature;

    public SerializableDictionary<string, KeyCode> keySettings = new();
    public float masterVol;
    public float bgmVol;
    public float sfxVol;

    public void ResetData(string playerName)
    {
        this.playerName = playerName;
        selectedQuickslot = 0;

        timeLeft = TimeManager.Instance.GetHalftime();
        isAM = true;
        day = 1;
        hour = 6;

        weather = WeatherType.Sunny;
        sky = SkyType.Sunrise;
        weatherActivationHour = 24;
        temperature = -5f;

        keySettings["Crouch"] = KeyCode.LeftShift;
        keySettings["Attack"] = KeyCode.LeftControl;
        keySettings["Jump"] = KeyCode.Space;

        keySettings["Inventory"] = KeyCode.I;
        keySettings["Interact"] = KeyCode.F;
        keySettings["Character"] = KeyCode.C;
        keySettings["Quest"] = KeyCode.J;

        keySettings["RRotate"] = KeyCode.E;
        keySettings["LRotate"] = KeyCode.Q;
        keySettings["Up"] = KeyCode.W;
        keySettings["Down"] = KeyCode.S;
        keySettings["Right"] = KeyCode.D;
        keySettings["Left"] = KeyCode.A;

        masterVol = 0;
        bgmVol = 0;
        sfxVol = 0;
    }
}
