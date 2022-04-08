using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Ű���� ���� �Է� ���� ���� ���� </summary>
public class DoubleKeyPress
{
    public KeyCode Key { get; private set; }

    /// <summary> �� �� ������ ������ ���� </summary>
    public bool SinglePressed { get; private set; }

    /// <summary> �� �� ������ ������ ���� </summary>
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

    /// <summary> MonoBehaviour.Update()���� ȣ�� : Ű ���� ������Ʈ </summary>
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

    /// <summary> MonoBehaviour.Update()���� ȣ�� : Ű �Է¿� ���� ���� </summary>
    public void UpdateAction(Action singlePressAction, Action doublePressAction)
    {
        if (SinglePressed) singlePressAction?.Invoke();
        if (DoublePressed) doublePressAction?.Invoke();
    }
}
    /*
    private DoubleKeyPressDetection[] keys;

    private void Start()
    {
        keys = new[]
        {
        new DoubleKeyPressDetection(KeyCode.W),
        new DoubleKeyPressDetection(KeyCode.A),
        new DoubleKeyPressDetection(KeyCode.S),
        new DoubleKeyPressDetection(KeyCode.D),
    };
    }

    private void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].Update();
        }

        keys[0].UpdateAction(() => Debug.Log("W"), () => Debug.Log("WW"));
        keys[1].UpdateAction(() => Debug.Log("A"), () => Debug.Log("AA"));
        keys[2].UpdateAction(() => Debug.Log("S"), () => Debug.Log("SS"));
        keys[3].UpdateAction(() => Debug.Log("D"), () => Debug.Log("DD"));
    }
    */
    