using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer; 
    
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    
    [Header("Mixer Parameters")]
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    private bool isMusicMuted = false;
    private bool isSFXMuted = false;
    private float musicVolumeBeforeMute;
    private float sfxVolumeBeforeMute;

    void Start()
    {
        InitializeSliders();
        
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void InitializeSliders()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.85f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.85f);
        
        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
            SetMusicVolume(savedMusicVolume);
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
            SetSFXVolume(savedSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        float volumeDB = volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
        
        if (audioMixer != null)
        {
            audioMixer.SetFloat(musicVolumeParameter, volumeDB);
        }
        
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        float volumeDB = volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
        
        if (audioMixer != null)
        {
            audioMixer.SetFloat(sfxVolumeParameter, volumeDB);
        }
        
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void ToggleMusicMute()
    {
        if (isMusicMuted)
        {
            SetMusicVolume(musicVolumeBeforeMute);
            if (musicSlider != null)
                musicSlider.value = musicVolumeBeforeMute;
            isMusicMuted = false;
        }
        else
        {
            if (musicSlider != null)
                musicVolumeBeforeMute = musicSlider.value;
            audioMixer.SetFloat(musicVolumeParameter, -80f);
            isMusicMuted = true;
        }
    }

    public void ToggleSFXMute()
    {
        if (isSFXMuted)
        {
            SetSFXVolume(sfxVolumeBeforeMute);
            if (sfxSlider != null)
                sfxSlider.value = sfxVolumeBeforeMute;
            isSFXMuted = false;
        }
        else
        {
            if (sfxSlider != null)
                sfxVolumeBeforeMute = sfxSlider.value;
            audioMixer.SetFloat(sfxVolumeParameter, -80f);
            isSFXMuted = true;
        }
    }

    public void MuteMusic()
    {
        if (!isMusicMuted)
        {
            if (musicSlider != null)
                musicVolumeBeforeMute = musicSlider.value;
            audioMixer.SetFloat(musicVolumeParameter, -80f);
            isMusicMuted = true;
        }
    }

    public void UnmuteMusic()
    {
        if (isMusicMuted)
        {
            SetMusicVolume(musicVolumeBeforeMute);
            if (musicSlider != null)
                musicSlider.value = musicVolumeBeforeMute;
            isMusicMuted = false;
        }
    }

    public void MuteSFX()
    {
        if (!isSFXMuted)
        {
            if (sfxSlider != null)
                sfxVolumeBeforeMute = sfxSlider.value;
            audioMixer.SetFloat(sfxVolumeParameter, -80f);
            isSFXMuted = true;
        }
    }

    public void UnmuteSFX()
    {
        if (isSFXMuted)
        {
            SetSFXVolume(sfxVolumeBeforeMute);
            if (sfxSlider != null)
                sfxSlider.value = sfxVolumeBeforeMute;
            isSFXMuted = false;
        }
    }

    public void ResetToDefault()
    {
        float defaultVolume = 0.75f;
        
        if (musicSlider != null)
        {
            musicSlider.value = defaultVolume;
            SetMusicVolume(defaultVolume);
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.value = defaultVolume;
            SetSFXVolume(defaultVolume);
        }

        // Resetar estados de mute
        isMusicMuted = false;
        isSFXMuted = false;
    }
}