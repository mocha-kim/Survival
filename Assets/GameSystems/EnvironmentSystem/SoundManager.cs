using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private AudioSource bgm;
    [SerializeField]
    private AudioSource envSFX;

    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField]
    private AudioClip[] sfxClips;

    public float minVolume = -20f;

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

    public void PlayBGM()
    {
        bgm.Play();
    }

    public void PauseBGM()
    {
        bgm.Pause();
    }

    public void PlaySFX(AudioClip clip)
    {
        envSFX.PlayOneShot(clip);
    }

    public void SetMasterVolume(float value)
    {
        float volume = (value == minVolume) ? -80f : value;
        mixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float value)
    {
        float volume = (value == minVolume) ? -80f : value;
        mixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float value)
    {
        float volume = (value == minVolume) ? -80f : value;
        mixer.SetFloat("SFXVolume", volume);
    }
}
