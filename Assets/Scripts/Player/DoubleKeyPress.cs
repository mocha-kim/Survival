using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DoubleKeyPress is used player's control
 * 
 * [source] : https://rito15.github.io/posts/unity-memo-double-key-press/
 */

public class DoubleKeyPress
{
    public KeyCode Key { get; private set; }
    public bool SinglePressed { get; private set; }
    public bool DoublePressed { get; private set; }

    private bool doublePressDetected;
    private float doublePressThreshold;
    private float lastKeyDownTime;

    public DoubleKeyPress(KeyCode key, float threshold = 0.3f)
    {
        this.Key = key;
        SinglePressed = false;
        DoublePressed = false;
        doublePressDetected = false;
        doublePressThreshold = threshold;
        lastKeyDownTime = 0f;
    }

    public void ChangeKey(KeyCode key)
    {
        this.Key = key;
    }
    public void ChangeThreshold(float seconds)
    {
        doublePressThreshold = seconds > 0f ? seconds : 0f;
    }

    public void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            doublePressDetected =
                (Time.time - lastKeyDownTime < doublePressThreshold);

            lastKeyDownTime = Time.time;
        }

        if (Input.GetKey(Key))
        {
            if (doublePressDetected)
                DoublePressed = true;
            else
                SinglePressed = true;
        }
        else
        {
            doublePressDetected = false;
            DoublePressed = false;
            SinglePressed = false;
        }
    }

    public void UpdateAction(Action singlePressAction, Action doublePressAction)
    {
        if (SinglePressed) singlePressAction?.Invoke();
        if (DoublePressed) doublePressAction?.Invoke();
    }
}