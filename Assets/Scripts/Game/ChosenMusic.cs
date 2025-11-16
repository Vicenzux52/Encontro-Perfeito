using UnityEngine;

public class ChosenMusic : MonoBehaviour
{
    public AudioClip[] radioClips;
    public AudioClip defaultClip;
    private int currentRadioIndex = 0;
    public AudioSource radioAudioSource;

    void Awake()
    {
        if (radioAudioSource == null)
        {
            Debug.LogError("AudioSource não atribuído!");
            return;
        }

        if (PlayerPrefs.HasKey("RadioMusicIndex"))
        {
            currentRadioIndex = PlayerPrefs.GetInt("RadioMusicIndex");

            if (radioClips.Length > 0)
            {
                radioAudioSource.clip = radioClips[currentRadioIndex];
                radioAudioSource.Play();
            }
            else
            {
                radioAudioSource.clip = defaultClip;
                radioAudioSource.Play();
            }
        }
        else
        {
            radioAudioSource.clip = defaultClip;
            radioAudioSource.Play();
        }

        radioAudioSource.loop = true;
    }
}
