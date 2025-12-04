using UnityEngine;

public class TriggerPhoto : MonoBehaviour
{
    public GameObject panel;
    public int iM, iP, iImg;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            panel.SetActive(true);

            PhonePanel phonePanel = panel.GetComponent<PhonePanel>();

            if (phonePanel != null)
            {
                phonePanel.SetupPanel(iM, iP, iImg);
            }

            Time.timeScale = 0f;
            hasBeenTriggered = true;

            gameObject.SetActive(false);
        }
    }
}