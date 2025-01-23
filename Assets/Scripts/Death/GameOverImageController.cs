using UnityEngine;
using UnityEngine.UI;

public class GameOverImageController : MonoBehaviour
{
    private Image gameOverImage;
    private float fadeSpeed = 1f; // Vitesse du fade-in
    private bool isFadingIn = false;

    private void Start()
    {
        // Récupère le composant Image
        gameOverImage = GetComponent<Image>();

        // Cache l'image au début
        if (gameOverImage != null)
        {
            gameOverImage.enabled = false;
            gameOverImage.color = new Color(1, 1, 1, 0); // Alpha à 0
        }
        else
        {
            Debug.LogWarning("Aucune image n'est attachée à ce GameObject !");
        }
    }

    private void Update()
    {
        // Vérifie si le joueur est mort
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            if (!isFadingIn)
            {
                StartFadeIn();
            }
        }

        // Gère l'effet de fade-in
        if (isFadingIn)
        {
            FadeIn();
        }
    }

    private void StartFadeIn()
    {
        if (gameOverImage != null)
        {
            gameOverImage.enabled = true; // Active l'image
            isFadingIn = true;
        }
    }

    private void FadeIn()
    {
        if (gameOverImage != null)
        {
            Color color = gameOverImage.color;
            color.a += fadeSpeed * Time.deltaTime; // Augmente l'alpha

            if (color.a >= 1f) // Si l'alpha atteint 1, termine le fade
            {
                color.a = 1f;
                isFadingIn = false;
            }

            gameOverImage.color = color;
        }
    }
}
