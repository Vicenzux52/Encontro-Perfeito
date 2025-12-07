using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PhotoData
{
    public Image photoF;
    public Image photoM;
    public Image maskImage;
    public Sprite[] photoParts;
}

public class PhotoAlbumManager : MonoBehaviour
{
    [Header("Photos")]
    public PhotoData[] photos;
    public Image[] desenhos;

    public static bool isGameOverTime = false;
    public Image gameOverTimeSprite;
    public Image gameOverTimeMask;

    public static bool isGameOverDeath = false;
    public Image gameOverDeathSprite;
    public Image gameOverDeathMask;

    public static bool isGameOverEsterEgg = false;
    public Image gameOverEsterEggSprite;
    public Image gameOverEsterEggMask;

    public static bool bunny = false;
    public Image bunnySprite;
    public Image bunnyMask;

    public static bool tortoise = false;
    public Image tortoiseSprite;
    public Image tortoiseMask;

    public static bool cat = false;
    public Image catSprite;
    public Image catMask;

    void Start()
    {
        UpdatePhotos();
    }

    void Update()
    {
        if (isGameOverTime)
        {
            gameOverTimeSprite.enabled = true;
            gameOverTimeMask.enabled = false;
        }
        else
        {
            gameOverTimeMask.enabled = true;
            gameOverTimeSprite.enabled = false;
        }

        if (isGameOverDeath)
        {
            gameOverDeathSprite.enabled = true;
            gameOverDeathMask.enabled = false;
        }
        else
        {
            gameOverDeathMask.enabled = true;
            gameOverDeathSprite.enabled = false;
        }

        if (isGameOverEsterEgg)
        {
            gameOverEsterEggSprite.enabled = true;
            gameOverEsterEggMask.enabled = false;
        }
        else
        {
            gameOverEsterEggMask.enabled = true;
            gameOverEsterEggSprite.enabled = false;
        }
        if (bunny)
        {
            bunnySprite.enabled = true;
            bunnyMask.enabled = false;
        }
        else
        {
            bunnyMask.enabled = true;
            bunnySprite.enabled = false;
        }
        if (cat)
        {
            catSprite.enabled = true;
            catMask.enabled = false;
        }
        else
        {
            catMask.enabled = true;
            catSprite.enabled = false;
        }
        if (tortoise)
        {
            tortoiseSprite.enabled = true;
            tortoiseMask.enabled = false;
        }
        else
        {
            tortoiseMask.enabled = true;
            tortoiseSprite.enabled = false;
        }
    }

    public void UpdatePhotos()
    {
        string paqueraEscolhida = PlayerPrefs.GetString("paqueraSelect", "Feminino");

        for (int i = 0; i < photos.Length; i++)
        {
            int collectedParts = CollectibleProgress.photoPartsCollected[i];

            bool isFeminino = paqueraEscolhida == "Feminino";
            photos[i].photoF.gameObject.SetActive(isFeminino);
            photos[i].photoM.gameObject.SetActive(!isFeminino);

            if (collectedParts < 3)
            {
                int partIndex = Mathf.Clamp(collectedParts, 0, photos[i].photoParts.Length - 1);
                photos[i].maskImage.sprite = photos[i].photoParts[partIndex];
                photos[i].maskImage.enabled = true;
            }
            else
            {
                photos[i].maskImage.enabled = false;
                desenhos[i].gameObject.SetActive(true);
            }
        }
    }
}
