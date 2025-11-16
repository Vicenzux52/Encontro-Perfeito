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

    public static bool isGameOver = false;
    public Image gameOverSprite;
    public Image gameOverMask;

    void Start()
    {
        UpdatePhotos();
    }

    void Update()
    {
        if (isGameOver)
        {
            gameOverSprite.enabled = true;
            gameOverMask.enabled = false;
        }
        else
        {
            gameOverMask.enabled = true;
            gameOverSprite.enabled = false;
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
