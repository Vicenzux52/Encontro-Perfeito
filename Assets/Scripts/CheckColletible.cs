using UnityEngine;
using UnityEngine.UI;

public class CheckColletible : MonoBehaviour
{
    public int index;
    public Sprite collectibleImage;
    public Sprite collectibleSilhouetteImage;
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = collectibleSilhouetteImage;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.collectibleCollected[index])
        {
            Debug.Log("detectei");
            gameObject.GetComponent<Image>().sprite = collectibleImage;
        }
    }
}
