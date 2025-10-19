using UnityEngine;

public class WinArea : MonoBehaviour
{
    public UIController uiController;

    public void Start()
    {
        uiController = FindFirstObjectByType<UIController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (uiController != null)
            {
                uiController.Win();
            }
        }
    }
}