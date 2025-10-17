using UnityEngine;
using UnityEngine.UI;

public class CheckColletible : MonoBehaviour
{
    [Header ("ImageIcon")]
    public int index;
    public Sprite collectibleImage;
    public Sprite collectibleSilhouetteImage;

    [Header("PhotoAlbum")]
    public PhotoAlbumManager photoAlbumManager;
    [HideInInspector] public int photoQuant;

    void Start()
    {
        gameObject.GetComponent<Image>().sprite = collectibleSilhouetteImage;
        photoAlbumManager = GetComponent<PhotoAlbumManager>();
    }

    void Update()
    {
        if (UIController.collectibleCollected[index])
        {
            gameObject.GetComponent<Image>().sprite = collectibleImage;
            photoQuant = index;
        }
    }
}
