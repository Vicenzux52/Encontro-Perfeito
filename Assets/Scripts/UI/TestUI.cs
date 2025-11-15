using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public Sprite coiso;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = coiso;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
