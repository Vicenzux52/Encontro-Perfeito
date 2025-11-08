using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public Slider MusicSlide;
    public Slider SFXSlide;

    void Start()
    {
        MusicSlide.onValueChanged.AddListener(SetMusicVolume);
        SFXSlide.onValueChanged.AddListener(SetSFXVolume);

        SetMusicVolume(PlayerPrefs.GetFloat("musicSlider", 1));
        SetSFXVolume(PlayerPrefs.GetFloat("sfxSlider", 1));
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("musicSlider", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("sfxSlider", value);
    }

    //music
    public void SetMusicSlider()
    {
        AudioListener.volume = MusicSlide.value;
        SaveMusicSlider();
    }

    public void SaveMusicSlider()
    {
        PlayerPrefs.SetFloat("musicSlider", MusicSlide.value);
    }

    public void LoadMusicSlider()
    {
        MusicSlide.value = PlayerPrefs.GetFloat("musicSlider");
    }

    public void MutedMusic(bool musicMuted)
    {
        if (musicMuted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    //sfx

    public void SetSFXSlider()
    {
        AudioListener.volume = SFXSlide.value;
        SaveSFXSlider();
    }

    public void SaveSFXSlider()
    {
        PlayerPrefs.SetFloat("sfxSlider", SFXSlide.value);
    }

    public void LoadSXFSlider()
    {
        SFXSlide.value = PlayerPrefs.GetFloat("sfxSlider");
    }

    public void MutedSFX(bool sfxMuted)
    {
        if (sfxMuted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
