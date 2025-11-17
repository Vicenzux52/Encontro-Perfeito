using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI Instance;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
