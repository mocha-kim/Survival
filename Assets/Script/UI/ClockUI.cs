using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    public Slider clock;

    public Image fill;
    public Color[] fillColors;
    public Image background;
    public Sprite[] backgroundSprits;

    private float TimeLeft => TimeManager.Instance.timeLeft;

    private void Start()
    {
        fill.color = fillColors[0];
        background.sprite = backgroundSprits[0];

        // init slider values
        clock.maxValue = TimeManager.Instance.halftimeOfDay * 2;
    }

    private void Update()
    {
        clock.value = TimeLeft;
    }

    public void ChangeBackgroundSprite(bool isDaytime)
    {
        fill.color = isDaytime ? fillColors[0] : fillColors[1];
        //background.sprite = isDaytime ? backgroundSprits[0] : backgroundSprits[1];
    }
}
