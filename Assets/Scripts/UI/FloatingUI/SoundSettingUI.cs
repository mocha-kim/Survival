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

    private float savedMasterVol = 0f;
    private float savedBGMVol = 0f;
    private float savedSFXVol = 0f;

    protected override void Awake()
    {
        // do nothing
    }

    private void Start()
    {
        masterVolText = masterSlider.GetComponentInChildren<TextMeshProUGUI>();
        bgmVolText = bgmSlider.GetComponentInChildren<TextMeshProUGUI>();
        sfxVolText = sfxSlider.GetComponentInChildren<TextMeshProUGUI>();

        masterSlider.value = GameManager.Instance.gameData.masterVol;
        bgmSlider.value = GameManager.Instance.gameData.bgmVol;
        sfxSlider.value = GameManager.Instance.gameData.sfxVol;

        OnMasterVolumeChanged();
        OnBGMVolumeChanged();
        OnSFXVolumeChanged();
    }

    public void OnMasterVolumeChanged()
    {
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
        masterVolText.text = SliderValueToVolumeText(masterSlider.value);
        if (!masterToggle.isOn)
        {
            masterToggle.isOn = true;
            OnMasterVolumeMuteToggle();
        }
    }

    public void OnBGMVolumeChanged()
    {
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
        bgmVolText.text = SliderValueToVolumeText(bgmSlider.value);
        if (!bgmToggle.isOn)
        {
            bgmToggle.isOn = true;
            OnBGMVolumeMuteToggle();
        }
    }

    public void OnSFXVolumeChanged()
    {
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);
        sfxVolText.text = SliderValueToVolumeText(sfxSlider.value);
        if (!sfxToggle.isOn)
        {
            sfxToggle.isOn = true;
            OnSFXVolumeMuteToggle();
        }
    }

    private string SliderValueToVolumeText(float value)
    {
        return ((value + 20) * 5).ToString("f0") + "%";
    }

    public void OnMasterVolumeMuteToggle()
    {
        if (masterToggle.isOn)
        {
            SoundManager.Instance.SetMasterVolume(savedMasterVol);
            SetMasterUI(savedMasterVol, SliderValueToVolumeText(savedMasterVol));
            bgmToggle.isOn = true;
            OnBGMVolumeMuteToggle();
            sfxToggle.isOn = true;
            OnSFXVolumeMuteToggle();
        }
        else
        {
            SoundManager.Instance.SetMasterVolume(SoundManager.Instance.minVolume);
            SetMasterUI(-20, "Mute");
            bgmToggle.isOn = false;
            OnBGMVolumeMuteToggle();
            sfxToggle.isOn = false;
            OnSFXVolumeMuteToggle();
        }
    }

    public void OnBGMVolumeMuteToggle()
    {
        if (bgmToggle.isOn)
        {
            SoundManager.Instance.SetBGMVolume(savedBGMVol);
            SetBGMUI(savedBGMVol, SliderValueToVolumeText(savedBGMVol));
        }
        else
        {
            SoundManager.Instance.SetBGMVolume(SoundManager.Instance.minVolume);
            SetBGMUI(-20, "Mute");
        }
    }

    public void OnSFXVolumeMuteToggle()
    {
        if (sfxToggle.isOn)
        {
            SoundManager.Instance.SetSFXVolume(savedSFXVol);
            SetSFXUI(savedSFXVol, SliderValueToVolumeText(savedSFXVol));
        }
        else
        {
            SoundManager.Instance.SetSFXVolume(SoundManager.Instance.minVolume);
            SetSFXUI(-20, "Mute");
        }
    }

    private void SetMasterUI(float volume, string text)
    {
        savedMasterVol = masterSlider.value;
        masterSlider.value = volume;
        masterVolText.text = text;
    }

    private void SetBGMUI(float volume, string text)
    {
        savedBGMVol = bgmSlider.value;
        bgmSlider.value = volume;
        bgmVolText.text = text;
    }

    private void SetSFXUI(float volume, string text)
    {
        savedSFXVol = sfxSlider.value;
        sfxSlider.value = volume;
        sfxVolText.text = text;
    }
}
