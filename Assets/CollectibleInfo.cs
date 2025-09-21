using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class CollectibleInfo : MonoBehaviour
{
    public int index;
    // public Sprite collectibleImage;
    // public Sprite collectibleSilhouetteImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (collectibleImage == null) Debug.LogError("Coletavel sem imagem");
        //UIController.RegisterCollectible(index, collectibleImage, collectibleSilhouetteImage);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        UIController.Collect(index);
    }
}
