using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckColletible : MonoBehaviour
{
    [Header("Collectible Icon UI")]
    public Image uiIcon;
    public int index;
    public Sprite collectibleImage;
    public Sprite collectibleSilhouetteImage;

    [Header("Collectible Area")]
    public GameObject collectible;

    [Header("PhotoAlbum")]
    public int photoIndex;

    [Header("Text Paquera")]
    public GameObject paqueraText;
    public float paqueraTextDuration = 2f;
    public GameObject PaqueraFText;
    public GameObject PaqueraMText;


    [Header("Audio Source")]
    public AudioSource audioSource;
    public AudioClip mensageNotification;

    private CarregarEscolhaPaqueras carregarEscolhaPaqueras;
    private bool playerInsideArea = false;
    private bool collected = false;

    void Start()
    {
        carregarEscolhaPaqueras = FindFirstObjectByType<CarregarEscolhaPaqueras>();

        if (uiIcon != null)
            uiIcon.sprite = collectibleSilhouetteImage;

        if (paqueraText != null)
            paqueraText.SetActive(false);

    }

    public void Collect()
    {
        if (collected) return;

        collected = true;

        CollectibleProgress.collectedItems[index] = true;

        if (photoIndex >= 0 && photoIndex < CollectibleProgress.photoPartsCollected.Length)
            CollectibleProgress.photoPartsCollected[photoIndex] = Mathf.Min(
                CollectibleProgress.photoPartsCollected[photoIndex] + 1, 3
            );

        if (uiIcon != null)
            uiIcon.sprite = collectibleImage;

        collectible.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.gameObject.CompareTag("CollectibleArea"))
        {
            playerInsideArea = true;
        }
        else if (other.gameObject == collectible && !collected)
        {
            Collect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.gameObject.CompareTag("CollectibleArea") && playerInsideArea)
        {
            if (!collected)
                StartCoroutine(ShowTextPaquera());

            playerInsideArea = false;
        }
    }


    IEnumerator ShowTextPaquera()
    {
        string paqueraSelecionada = (carregarEscolhaPaqueras != null)
            ? carregarEscolhaPaqueras.paquera
            : PlayerPrefs.GetString("paqueraSelect", "Feminino");

        PaqueraFText.SetActive(paqueraSelecionada == "Feminino");
        PaqueraMText.SetActive(paqueraSelecionada == "Masculino");

        paqueraText.SetActive(true);
        audioSource.PlayOneShot(mensageNotification);

        yield return new WaitForSeconds(paqueraTextDuration);

        paqueraText.SetActive(false);
        PaqueraFText.SetActive(false);
        PaqueraMText.SetActive(false);
    }
}