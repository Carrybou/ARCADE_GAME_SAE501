using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton pour un accès global

    [System.Serializable]
    public class Sound
    {
        public string name; // Nom du son (ex: "Explosion")
        public AudioClip clip; // Fichier audio associé
        [Range(0f, 1f)] public float volume = 1f; // Volume du son
        public bool loop = false; // Si le son doit boucler
    }

    public List<Sound> sounds; // Liste des sons à gérer
    private Dictionary<string, AudioSource> soundDictionary; // Dictionnaire pour un accès rapide

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Préserver l'AudioManager entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soundDictionary = new Dictionary<string, AudioSource>();

        // Créer des AudioSources pour chaque son
        foreach (var sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = sound.loop;

            soundDictionary.Add(sound.name, source);
        }
    }

    // Méthode pour jouer un son
    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out AudioSource source))
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioManager: Aucun son trouvé avec le nom '{name}'");
        }
    }

    // Méthode pour arrêter un son
    public void StopSound(string name)
    {
        if (soundDictionary.TryGetValue(name, out AudioSource source))
        {
            source.Stop();
        }
    }

    public AudioSource GetAudioSource(string name)
{
    if (soundDictionary.TryGetValue(name, out AudioSource source))
    {
        return source;
    }
    return null;
}
public void IncreasePitch(string name, float pitchIncreaseRate, float maxPitch)
{
    if (soundDictionary.TryGetValue(name, out AudioSource source))
    {
        float newPitch = source.pitch + pitchIncreaseRate * Time.deltaTime;
        source.pitch = Mathf.Clamp(newPitch, 1.0f, maxPitch);
    }
}
public IEnumerator FadeInSound(string name, float targetVolume, float duration)
{
    if (soundDictionary.TryGetValue(name, out AudioSource source))
    {
        source.volume = 0f; // Commence avec un volume nul
        source.Play(); // Joue le son

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / duration);
            yield return null; // Attend le prochain frame
        }

        source.volume = targetVolume; // Assure que le volume atteint bien la valeur cible
    }
}


}
