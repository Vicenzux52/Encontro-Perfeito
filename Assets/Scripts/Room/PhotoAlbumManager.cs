using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PhotoData
{
    public Image photoImage;
    public Sprite[] photoParts;
    //[HideInInspector] public int collectedParts = 0;
}

public class PhotoAlbumManager : MonoBehaviour
{
    [Header("Photos")]
    public PhotoData[] photos;

    void Start()
    {
        UpdatePhotos();
    }

    public void UpdatePhotos()
    {
        for (int i = 0; i < photos.Length; i++)
        {
            int collectedParts = CollectibleProgress.photoPartsCollected[i];

            if (collectedParts >= 3)
            {
                photos[i].photoImage.enabled = false;
            }
            else
            {
                photos[i].photoImage.enabled = true;
                int partIndex = Mathf.Clamp(collectedParts, 0, photos[i].photoParts.Length - 1);
                photos[i].photoImage.sprite = photos[i].photoParts[partIndex];
            }
        }
    }
}