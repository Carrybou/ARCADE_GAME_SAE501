using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isGameOver = false;
    public bool isDoublePointsActive = false;

    public int score = 0;
    private bool hasRestarted = false;

    public bool isSlowMotionActive = false;
    private float slowMotionFactor = 0.7f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
            value *= 2;
        }

        score += value;
    }


    public void StopGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("[GameManager] Fin du jeu - Score final : " + score);

        StartCoroutine(CheckAndSubmitHighscore());
    }

    private IEnumerator CheckAndSubmitHighscore()
    {
        // Vérifie si les highscores sont déjà chargés, sinon les récupérer
        if (!Anatidae.HighscoreManager.HasFetchedHighscores)
        {
            Debug.Log("[GameManager] Les highscores ne sont pas chargés, récupération en cours...");
            yield return StartCoroutine(Anatidae.HighscoreManager.FetchHighscores());
        }

        // Vérifie à nouveau après la récupération
        if (!Anatidae.HighscoreManager.HasFetchedHighscores)
        {
            Debug.LogError("[GameManager] Impossible de vérifier les highscores, la récupération a échoué.");
            yield break;
        }

        if (Anatidae.HighscoreManager.IsHighscore(score))
        {
            Debug.Log("[GameManager] ✅ Nouveau highscore détecté ! Affichage du formulaire...");

            // 🔹 Réinitialisation du PlayerName pour obliger la saisie d'un nouveau nom
            Anatidae.HighscoreManager.PlayerName = null;

            // ✅ Remplace l’accès direct par l’utilisation de la méthode publique
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
        if (hasRestarted) return;

        Debug.Log("[GameManager] 🔄 Redémarrage en cours...");
        hasRestarted = true;

        Destroy(Instance.gameObject);
        Instance = null;

        // Recharge la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isGameOver = false;
        score = 0;

        Anatidae.HighscoreManager.PlayerName = null;
    }

    public float GetSlowMotionMultiplier()
    {
        return isSlowMotionActive ? slowMotionFactor : 1f; // Si actif, réduit la vitesse
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
