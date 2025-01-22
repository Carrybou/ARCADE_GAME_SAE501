using UnityEngine;

public class GameOverAnimation : MonoBehaviour
{
    public RectTransform gameOverImage; // L'image du Game Over
    public AnimationCurve growCurve; // Courbe d'animation pour l'agrandissement initial
    public float growDuration = 1.5f; // Temps pour atteindre la taille finale
    public float pulseScale = 1.02f; // Facteur d'oscillation (1.02 = +2%)
    public float pulseSpeed = 1.5f; // Vitesse de l'oscillation

    private Vector2 initialSize = Vector2.zero; // Taille initiale de l'image
    private Vector2 finalSize = new Vector2(1920, 1080); // Taille finale de l'image
    private float elapsedTime = 0f; // Temps écoulé pour l'animation initiale
    private bool isInitialAnimationComplete = false; // Si l'agrandissement initial est terminé
    private float pulseTimer = 0f; // Timer pour l'oscillation

    void Start()
    {
        if (gameOverImage == null)
        {
            Debug.LogError("Aucun RectTransform assigné ! Assignez une image dans l’inspecteur.");
            return;
        }

        if (growCurve == null)
        {
            Debug.LogError("Aucune courbe d'animation assignée ! Assignez une courbe dans l’inspecteur.");
            return;
        }

        // Initialiser la taille à 0 (ou celle spécifiée)
        gameOverImage.sizeDelta = initialSize;
    }

    void Update()
    {
        if (!isInitialAnimationComplete)
        {
            // Animation d'agrandissement initial
            AnimateToFinalSize();
        }
        else
        {
            // Animation continue (pulsation)
            PulseAnimation();
        }
    }

    private void AnimateToFinalSize()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / growDuration);
        float curveValue = growCurve.Evaluate(t);
        gameOverImage.sizeDelta = Vector2.LerpUnclamped(initialSize, finalSize, curveValue);

        // Si la taille finale est atteinte
        if (t >= 1.0f)
        {
            isInitialAnimationComplete = true; // Passe à l'animation continue
        }
    }

    private void PulseAnimation()
    {
        // Création d'une pulsation basée sur Sinus
        pulseTimer += Time.deltaTime * pulseSpeed;
        float scaleFactor = 1.0f + Mathf.Sin(pulseTimer) * (pulseScale - 1.0f);

        // Appliquer le facteur de taille à l'image
        gameOverImage.sizeDelta = finalSize * scaleFactor;
    }
}