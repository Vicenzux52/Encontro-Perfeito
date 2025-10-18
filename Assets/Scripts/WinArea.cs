using UnityEngine;

public class WinArea : MonoBehaviour
{
    public UIController uiController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiController.Win();
        }
    }
}
