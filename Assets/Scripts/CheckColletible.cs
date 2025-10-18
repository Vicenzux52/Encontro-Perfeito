using UnityEngine;
using UnityEngine.UI;

public class CheckColletible : MonoBehaviour
{
    [Header("Collectible Icon UI")]
    public Image uiIcon;
    public int index;
    public Sprite collectibleImage;
    public Sprite collectibleSilhouetteImage;

    [Header("PhotoAlbum")]
    public int photoIndex;

    private bool addedToProgress = false;

    void Start()
    {
        if (uiIcon != null)
            uiIcon.sprite = collectibleSilhouetteImage;
    }

    public void Collect()
    {
        if (!addedToProgress)
        {
            CollectibleProgress.collectedItems[index] = true;

            if (photoIndex >= 0 && photoIndex < CollectibleProgress.photoPartsCollected.Length)
                CollectibleProgress.photoPartsCollected[photoIndex] = Mathf.Min(
                    CollectibleProgress.photoPartsCollected[photoIndex] + 1, 3
                );

            if (uiIcon != null)
                uiIcon.sprite = collectibleImage;

            addedToProgress = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
}