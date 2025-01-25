using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HurtOverlayController : MonoBehaviour
{
    private Image hurtOverlay;

    void Start()
    {
        hurtOverlay = GetComponent<Image>();
    }

    public void FlashOverlay(float seconds, float intensity)
    {
        StartCoroutine(FlashCoroutine(seconds, intensity));
    }

    public void FadeOverlay(float seconds, float intensity)
    {
        StartCoroutine(FadeCoroutine(seconds, intensity));
    }

    private IEnumerator FlashCoroutine(float seconds, float intensity)
    {
        float elapsedTime = 0f;
        Color originalColor = hurtOverlay.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, intensity);

        // Fade in
        while (elapsedTime < seconds / 2)
        {
            hurtOverlay.color = Color.Lerp(originalColor, targetColor, elapsedTime / (seconds / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the target color is set
        hurtOverlay.color = targetColor;

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < seconds / 2)
        {
            hurtOverlay.color = Color.Lerp(targetColor, originalColor, elapsedTime / (seconds / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the original color is set
        hurtOverlay.color = originalColor;
    }

    private IEnumerator FadeCoroutine(float seconds, float intensity)
    {
        float elapsedTime = 0f;
        Color originalColor = hurtOverlay.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, intensity);

        // Fade in
        while (elapsedTime < seconds)
        {
            hurtOverlay.color = Color.Lerp(originalColor, targetColor, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the target color is set
        hurtOverlay.color = targetColor;
    }
}