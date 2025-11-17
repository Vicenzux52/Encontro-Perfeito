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
    
    [Header("Toggles")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    [Header("Mixer Parameters")]
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    private float musicVolumeBeforeMute;
    private float sfxVolumeBeforeMute;

    void Start()
    {
        LoadVolumes();
        
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(ToggleMusic);
        }
        
        if (sfxToggle != null)
        {
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }
    }

    public void SetMusicVolume(float value)
    {
        float volume = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        
        bool success = audioMixer.SetFloat(musicVolumeParameter, volume);
        
        if (!success)
        {
            Debug.LogWarning($"Parâmetro '{musicVolumeParameter}' não encontrado no AudioMixer. Verifique o nome.");
        }
        
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.SetInt("MusicMuted", 0);
        
        if (musicToggle != null && value > 0)
        {
            musicToggle.isOn = true;
        }
    }

    public void SetSFXVolume(float value)
    {
        float volume = value > 0 ? Mathf.Log10(value) * 20 : -80f;
        
        bool success = audioMixer.SetFloat(sfxVolumeParameter, volume);
        
        if (!success)
        {
            Debug.LogWarning($"Parâmetro '{sfxVolumeParameter}' não encontrado no AudioMixer. Verifique o nome.");
        }
        
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.SetInt("SFXMuted", 0);
        
        if (sfxToggle != null && value > 0)
        {
            sfxToggle.isOn = true;
        }
    }

    public void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            SetMusicVolume(savedVolume);
        }
        else
        {
            if (musicSlider != null)
            {
                musicVolumeBeforeMute = musicSlider.value;
            }
            audioMixer.SetFloat(musicVolumeParameter, -80f);
            PlayerPrefs.SetInt("MusicMuted", 1);
        }
        
        PlayerPrefs.Save();
    }

    public void ToggleSFX(bool isOn)
    {
        if (isOn)
        {
            float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            SetSFXVolume(savedVolume);
        }
        else
        {
            if (sfxSlider != null)
            {
                sfxVolumeBeforeMute = sfxSlider.value;
            }
            audioMixer.SetFloat(sfxVolumeParameter, -80f);
            PlayerPrefs.SetInt("SFXMuted", 1);
        }
        
        PlayerPrefs.Save();
    }

    private void LoadVolumes()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        bool musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
        }
        
        if (musicToggle != null)
        {
            musicToggle.isOn = !musicMuted;
        }

        if (musicMuted)
        {
            audioMixer.SetFloat(musicVolumeParameter, -80f);
        }
        else
        {
            SetMusicVolume(musicVolume);
        }

        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        bool sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
        }
        
        if (sfxToggle != null)
        {
            sfxToggle.isOn = !sfxMuted;
        }

        if (sfxMuted)
        {
            audioMixer.SetFloat(sfxVolumeParameter, -80f);
        }
        else
        {
            SetSFXVolume(sfxVolume);
        }
    }

    public void PrintMixerParameters()
    {
        Debug.Log("=== AudioMixer Parameters ===");
        
        float musicValue;
        if (audioMixer.GetFloat(musicVolumeParameter, out musicValue))
        {
            Debug.Log($"{musicVolumeParameter}: {musicValue}");
        }
        else
        {
            Debug.LogError($"Parâmetro '{musicVolumeParameter}' não encontrado!");
        }
        
        float sfxValue;
        if (audioMixer.GetFloat(sfxVolumeParameter, out sfxValue))
        {
            Debug.Log($"{sfxVolumeParameter}: {sfxValue}");
        }
        else
        {
            Debug.LogError($"Parâmetro '{sfxVolumeParameter}' não encontrado!");
        }
    }
}