using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isGameOver = false;
    public bool isDoublePointsActive = false;
    public int score = 0;
    private bool hasRestarted = false; // Empêche un double restart

    [Header("Game Over Sound Effects")]
    public AudioClip gameOverVoiceClip;  // 🎙️ Voix "Game Over"
    public AudioClip gameOverFXClip;     // 🔊 Effet sonore Game Over

    private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 🔹 Réassigner les AudioClips au cas où ils ont été perdus après un restart
        if (gameOverVoiceClip == null || gameOverFXClip == null)
        {
            Debug.LogWarning("[GameManager] ⚠️ Réassignation des sons depuis Resources...");
            gameOverVoiceClip = Resources.Load<AudioClip>("Audio/game-over-man-vocal-spoken");
            gameOverFXClip = Resources.Load<AudioClip>("Audio/game-over-video-game-type-fx_200bpm_B_major");
        }
    }
    else
    {
        Destroy(gameObject);
        return;
    }
}


    public void AddScore(int value)
    {
        if (isDoublePointsActive)
        {
            value *= 2; // Double les points si le bonus est actif
        }

        score += value;
    }

    public void StopGame()
    {
        if (isGameOver) return; // Empêche plusieurs appels à StopGame

        isGameOver = true;
        Debug.Log("[GameManager] Fin du jeu - Score final : " + score);

        // 🔊 Jouer les sons de Game Over
        PlayGameOverSounds();

        StartCoroutine(CheckAndSubmitHighscore());
    }

    private void PlayGameOverSounds()
    {
        if (gameOverVoiceClip == null)
        {
            Debug.LogError("[GameManager] ❌ ERREUR : gameOverVoiceClip est NULL. Assurez-vous qu'il est bien assigné dans l'Inspector.");
            return;
        }

        if (gameOverFXClip == null)
        {
            Debug.LogError("[GameManager] ❌ ERREUR : gameOverFXClip est NULL. Assurez-vous qu'il est bien assigné dans l'Inspector.");
            return;
        }

        // Vérifie si Camera.main est bien assignée
        Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : Vector3.zero;

        // 🔊 Ajuste les volumes (1.0f = volume max, 0.0f = muet)
        float voiceVolume = 2f;  // Volume augmenté à 120%
        float fxVolume = 0.2f;     // Volume diminué à 50%

        // Jouer le son vocal (avec volume augmenté)
        AudioSource.PlayClipAtPoint(gameOverVoiceClip, soundPosition, Mathf.Clamp(voiceVolume, 0f, 1f));
        Debug.Log("[GameManager] 🎙️ Son vocal du Game Over joué avec volume augmenté !");

        // Jouer l'effet sonore (avec volume diminué)
        AudioSource.PlayClipAtPoint(gameOverFXClip, soundPosition, Mathf.Clamp(fxVolume, 0f, 1f));
        Debug.Log("[GameManager] 🔊 Effet sonore du Game Over joué avec volume réduit !");
    }





    private IEnumerator CheckAndSubmitHighscore()
    {
        if (!Anatidae.HighscoreManager.HasFetchedHighscores)
        {
            Debug.Log("[GameManager] Les highscores ne sont pas chargés, récupération en cours...");
            yield return StartCoroutine(Anatidae.HighscoreManager.FetchHighscores());
        }

        if (!Anatidae.HighscoreManager.HasFetchedHighscores)
        {
            Debug.LogError("[GameManager] Impossible de vérifier les highscores, la récupération a échoué.");
            yield break;
        }

        if (Anatidae.HighscoreManager.IsHighscore(score))
        {
            Debug.Log("[GameManager] ✅ Nouveau highscore détecté ! Affichage du formulaire...");
            
            Anatidae.HighscoreManager.PlayerName = null;
            Anatidae.HighscoreManager.ShowHighscoreInput(score);
        }
        else
        {
            Debug.Log("[GameManager] Score trop bas, pas d'enregistrement.");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("P1_Start"))
        {
            Debug.Log("[GameManager] Redémarrage via touche 'Z' du clavier azerty et w clavier qwerty...");
            RestartGame();
        }
    }

    public void RestartGame()
    {
        if (hasRestarted) return; // Empêche plusieurs resets simultanés

        Debug.Log("[GameManager] 🔄 Redémarrage en cours...");
        hasRestarted = true;

        Destroy(Instance.gameObject);
        Instance = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isGameOver = false;
        score = 0;

        Anatidae.HighscoreManager.PlayerName = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
