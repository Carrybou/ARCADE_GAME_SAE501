using System.Collections;
using UnityEngine;

public class CanvasAutoHider : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Référence au CanvasGroup pour contrôler l'opacité
    public float fadeDuration = 1.0f; // Durée de la transition (fade-in/out)
    public float hiddenDuration = 10.0f; // Temps pendant lequel le Canvas reste caché
    public float visibleDuration = 5.0f; // Temps pendant lequel le Canvas reste visible
    public float initialDelay = 2.0f; // Délai avant le premier affichage

    private void Start()
    {
        // S'assurer que le Canvas commence caché
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Démarrer la coroutine avec un délai initial
        StartCoroutine(AutoToggleCanvas());
    }

    private IEnumerator AutoToggleCanvas()
    {
        // Attendre le délai initial avant de commencer
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // Fade-in (réafficher le Canvas)
            yield return StartCoroutine(FadeCanvas(0.0f, 1.0f));
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            // Canvas visible (attendre la durée visible)
            yield return new WaitForSeconds(visibleDuration);

            // Fade-out (masquer le Canvas)
            yield return StartCoroutine(FadeCanvas(1.0f, 0.0f));
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            // Canvas caché (attendre la durée cachée)
            yield return new WaitForSeconds(hiddenDuration);
        }
    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha; // S'assurer que l'alpha final est correctement appliqué
    }
}
