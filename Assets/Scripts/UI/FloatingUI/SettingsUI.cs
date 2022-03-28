using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : Interface
{
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;


    public void OnMasterVolumeChanged()
    {
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
    }

    public void OnBGMVolumeChanged()
    {
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
    }

    public void OnSFXVolumeChanged()
    {
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    public void OnMasterVolumeMute()
    {
        SoundManager.Instance.SetMasterVolume(SoundManager.Instance.minVolume);
    }

    public void OnBGMVolumeMute()
    {
        SoundManager.Instance.SetBGMVolume(SoundManager.Instance.minVolume);
    }

    public void OnSFXVolumeMute()
    {
        SoundManager.Instance.SetSFXVolume(SoundManager.Instance.minVolume);
    }
}
