using UnityEngine;

public class AudioFade : MonoBehaviour
{
    private AudioSource audioSource;
    public float fadeDuration = 2.0f; // Durée du fondu en secondes
    private bool isFadingOut = false;
    private bool isFadingIn = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isFadingOut)
        {
            // Réduction du volume progressivement
            audioSource.volume -= Time.deltaTime / fadeDuration;
            if (audioSource.volume <= 0)
            {
                audioSource.volume = 0;
                isFadingOut = false;
                audioSource.Stop(); // Optionnel, si tu veux arrêter la musique après le fondu
            }
        }

        if (isFadingIn)
        {
            // Augmentation du volume progressivement
            audioSource.volume += Time.deltaTime / fadeDuration;
            if (audioSource.volume >= 1)
            {
                audioSource.volume = 1;
                isFadingIn = false;
            }
        }
    }

    // Fonction pour démarrer le fondu en sortie
    public void StartFadeOut()
    {
        if (!isFadingOut)
        {
            isFadingOut = true;
            audioSource.Play();  // Si la musique n'est pas encore jouée, tu peux démarrer ici
        }
    }

    // Fonction pour démarrer le fondu en entrée
    public void StartFadeIn()
    {
        if (!isFadingIn)
        {
            isFadingIn = true;
            audioSource.Play();  // Si la musique n'est pas encore jouée, tu peux démarrer ici
        }
    }

    // Fonction pour arrêter immédiatement la musique
    public void StopAudio()
    {
        audioSource.Stop();
    }
}
