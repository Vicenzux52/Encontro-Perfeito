using UnityEngine;

public class PhotoAlbumManager : MonoBehaviour
{
    [Header("CheckCollectible Script")]
    public CheckColletible checkColletible;

    [Header("Photos")]
    public GameObject[] photos;

    void Start()
    {
        if (checkColletible == null)
        {
            checkColletible = GetComponent<CheckColletible>();
        }
        
        foreach (GameObject photo in photos)
        {
            photo.SetActive(false);
        }
    }

    void Update()
    {
        int photoIndex = checkColletible.photoQuant;

        for (int i = 0; i < photos.Length; i++)
        {
            photos[i].SetActive(i == photoIndex);
        }
    }
}
