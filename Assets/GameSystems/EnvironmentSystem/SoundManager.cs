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
        GameManager.Instance.OnGameManagerStart += GetSoundData;
    }

    private void GetSoundData()
    {
        SetMasterVolume(GameManager.Instance.gameData.masterVol);
        SetBGMVolume(GameManager.Instance.gameData.bgmVol);
        SetSFXVolume(GameManager.Instance.gameData.sfxVol);
    }

    public void PlayBGM()
    {
        bgm.Play();
    }

    public void PauseBGM()
    {
        bgm.Pause();
    }

    public void PlaySFX(SFXType type)
    {
        if ((int)type < sfxClips.Length)
            envSFX.PlayOneShot(sfxClips[(int)type]);
    }

    public void SetMasterVolume(float value)
    {
        GameManager.Instance.gameData.masterVol = value;
        float volume = (value == minVolume) ? -80f : value;
        mixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float value)
    {
        GameManager.Instance.gameData.bgmVol = value;
        float volume = (value == minVolume) ? -80f : value;
        mixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float value)
    {
        GameManager.Instance.gameData.sfxVol = value;
        float volume = (value == minVolume) ? -80f : value;
        mixer.SetFloat("SFXVolume", volume);
    }
}
