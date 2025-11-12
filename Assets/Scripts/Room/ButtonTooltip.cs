using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipBox;
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;

    [Header("Fade Settings")]
    public float fadeDuration = 0.2f;
    public float hideDelay = 0.1f;

    void Start()
    {
        // Garante que o objeto tenha um CanvasGroup
        canvasGroup = tooltipBox.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = tooltipBox.AddComponent<CanvasGroup>();

        tooltipBox.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        tooltipBox.SetActive(true);
        fadeCoroutine = StartCoroutine(FadeTooltip(1f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideCoroutine = StartCoroutine(HideAfterDelay(hideDelay));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeTooltip(0f));
    }

    IEnumerator FadeTooltip(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
            tooltipBox.SetActive(false);
    }
}
