using UnityEngine;
using System.Collections;

public class WinArea : MonoBehaviour
{
    public UIController uiController;
    public GameObject paqueraF;
    public GameObject paqueraM;
    public float moveSpeed = 3f;
    public float stopDistance = 1f;

    private bool isMovingToPaquera = false;
    private GameObject player;
    private GameObject targetPaquera;

    public Animator paqueraFAnim;
    public Animator paqueraMAnim;

    void Start()
    {
        uiController = FindFirstObjectByType<UIController>();
        paqueraFAnim.SetBool("Wave", false);
        paqueraMAnim.SetBool("Wave", false);
        paqueraFAnim.SetBool("Idle", true);
        paqueraMAnim.SetBool("Idle", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMovingToPaquera)
        {
            player = other.gameObject;
            isMovingToPaquera = true;

            var playerController = player.GetComponent<Player>();
            if (playerController != null)
                playerController.enabled = false;

            if (paqueraF != null && paqueraF.activeInHierarchy)
                targetPaquera = paqueraF;
            else if (paqueraM != null && paqueraM.activeInHierarchy)
                targetPaquera = paqueraM;
            else
            {
                Debug.LogWarning("Nenhuma paquera ativa encontrada!");
                return;
            }

            StartCoroutine(MoveToPaquera());
        }
    }

    IEnumerator MoveToPaquera()
    {
        while (Vector3.Distance(player.transform.position, targetPaquera.transform.position) > stopDistance)
        {
            paqueraFAnim.SetBool("Idle", false);
            paqueraMAnim.SetBool("Idle", false);
            paqueraFAnim.SetBool("Wave", true);
            paqueraMAnim.SetBool("Wave", true);
            Debug.Log("Wave" + paqueraFAnim.GetBool("Wave"));

            Vector3 direction = (targetPaquera.transform.position - player.transform.position).normalized;
            player.transform.position += direction * moveSpeed * Time.deltaTime;

            Vector3 lookPos = new Vector3(targetPaquera.transform.position.x, player.transform.position.y, targetPaquera.transform.position.z);
            player.transform.LookAt(lookPos);

            yield return null;
        }

        if (uiController != null)
        {
            uiController.Win();
        }
    }
}
