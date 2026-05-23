using System.Collections;
using UnityEngine;

public class WindowFadeIn : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeInDuration = .5f;
    public float fadeOutDuration = .5f;

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    private IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1f;
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        gameObject.SetActive(false);
    }
}
