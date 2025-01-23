using UnityEngine;
using TMPro; // Nécessaire pour utiliser TextMeshPro

public class SmoothTextTransitionTMP : MonoBehaviour
{
    public TextMeshProUGUI textToFade; // L'élément TextMeshPro
    public float fadeDuration = 1.0f; // Durée d'apparition/disparition (en secondes)
    public float delayBetweenFades = 0.5f; // Temps entre chaque transition

    private bool isFadingIn = true; // Indique si le texte est en train d'apparaître
    private float timer = 0f; // Chronomètre pour la transition

    void Start()
    {
        if (textToFade == null)
        {
            textToFade = GetComponent<TextMeshProUGUI>(); // Récupère automatiquement le TextMeshPro s'il est sur le même GameObject
            if (textToFade == null)
            {
                Debug.LogError("Aucun TextMeshProUGUI trouvé ! Assigne-le dans l'inspecteur ou attache ce script à un élément contenant TextMeshPro.");
                return;
            }
        }

        // Initialiser le texte avec une transparence de 0 (invisible)
        SetTextAlpha(0);
    }

    void Update()
    {
        if (textToFade == null) return;

        // Met à jour le chronomètre
        timer += Time.deltaTime;

        // Transition Smooth
        if (isFadingIn && timer <= fadeDuration)
        {
            // Faire apparaître le texte
            float alpha = timer / fadeDuration;
            SetTextAlpha(alpha);
        }
        else if (!isFadingIn && timer <= fadeDuration)
        {
            // Faire disparaître le texte
            float alpha = 1 - (timer / fadeDuration);
            SetTextAlpha(alpha);
        }
        else if (timer > fadeDuration + delayBetweenFades)
        {
            // Réinitialise le chronomètre et change l'état
            timer = 0f;
            isFadingIn = !isFadingIn;
        }
    }

    // Change l'opacité (alpha) du texte
    private void SetTextAlpha(float alpha)
    {
        Color color = textToFade.color;
        color.a = Mathf.Clamp01(alpha); // S'assurer que la valeur est entre 0 et 1
        textToFade.color = color;
    }
}
