using UnityEngine;

public class PopupDisappears : MonoBehaviour
{
    [Tooltip("Temps avant disparition (en secondes).")]
    public float disappearDelay = 3f; // Temps avant la disparition

    private CanvasGroup canvasGroup;

    void Start()
    {
        // Récupérer ou ajouter un CanvasGroup pour gérer la transparence et l'interactivité
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Lancer la coroutine pour cacher la popup
        Invoke(nameof(HidePopup), disappearDelay);
    }

    void HidePopup()
    {
        // Désactiver visuellement et rendre le GameObject inutilisable
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float fadeDuration = 1f; // Durée de la transition
        float startAlpha = canvasGroup.alpha;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        gameObject.SetActive(false); // Désactiver complètement la popup
    }
}
