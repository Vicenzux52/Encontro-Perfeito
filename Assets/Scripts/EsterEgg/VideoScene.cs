using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (DeathSaver.estereggConcluido)
        {
            SceneManager.LoadScene(DeathSaver.returnScene);
        }
        else if (DeathSaver.estereggNaoConcluido)
        {
            SceneManager.LoadScene("Room");
        }
        else
        {
            SceneManager.LoadScene("EsterEgg");
        }
    }
}
