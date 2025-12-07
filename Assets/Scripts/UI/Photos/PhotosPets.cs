using UnityEngine;

public class PhotosPets : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TriggerPhotoCat")
        {
            PhotoAlbumManager.cat = true;
        }
        if (other.gameObject.name == "TriggerPhotoRabbit")
        {
            PhotoAlbumManager.bunny = true;
        }
        if (other.gameObject.name == "TriggerPhotoTortoise")
        {
            PhotoAlbumManager.tortoise = true;
        }
    }
}
