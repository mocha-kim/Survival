using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettingUI : UserInterface
{
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Toggle masterToggle;
    [SerializeField]
    private Toggle bgmToggle;
    [SerializeField]
    private Toggle sfxToggle;

    private TextMeshProUGUI masterVolText;
    private TextMeshProUGUI bgmVolText;
    private TextMeshProUGUI sfxVolText;

    private void Start()
    {
        masterVolText = masterSlider.GetComponentInChildren<TextMeshProUGUI>();
        bgmVolText = bgmSlider.GetComponentInChildren<TextMeshProUGUI>();
        sfxVolText = sfxSlider.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnMasterVolumeChanged()
    {
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
        masterVolText.text = ((masterSlider.value + 20) * 5).ToString("f0") + "%";
    }

    public void OnBGMVolumeChanged()
    {
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
        bgmVolText.text = ((bgmSlider.value + 20) * 5).ToString("f0") + "%";
    }

    public void OnSFXVolumeChanged()
    {
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);
        sfxVolText.text = ((sfxSlider.value + 20) * 5).ToString("f0") + "%";
    }

    public void OnMasterVolumeMute()
    {
        if (masterToggle.isOn)
        {
            SoundManager.Instance.SetMasterVolume(masterSlider.value);
        }
        else
        {
            SoundManager.Instance.SetMasterVolume(SoundManager.Instance.minVolume);
        }
    }

    public void OnBGMVolumeMute()
    {
        if (bgmToggle.isOn)
        {
            SoundManager.Instance.SetBGMVolume(masterSlider.value);
        }
        else
        {
            SoundManager.Instance.SetBGMVolume(SoundManager.Instance.minVolume);
        }
    }

    public void OnSFXVolumeMute()
    {
        if (sfxToggle.isOn)
        {
            SoundManager.Instance.SetSFXVolume(masterSlider.value);
        }
        else
        {
            SoundManager.Instance.SetSFXVolume(SoundManager.Instance.minVolume);
        }
    }
}
