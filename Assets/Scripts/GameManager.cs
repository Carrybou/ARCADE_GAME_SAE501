using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Nécessaire pour la gestion des scènesw

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isGameOver = false;
    public int score = 0;
   
    
    
    void Start()
    {
         StartCoroutine(AudioManager.Instance.FadeInSound("Music", 1.0f, 3.0f)); // Fade-in sur 3 secondes
    }
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
        score += value;
        Debug.Log("Score : " + score);
    }

    public void StopGame()
    {
        if (isGameOver)
        {
            return; // Empêche plusieurs appels à StopGame
        }
       
        isGameOver = true;

        // Affiche le menu pour entrer le high score
        Anatidae.HighscoreManager.ShowHighscoreInput(score);
        // if (Anatidae.HighscoreManager.IsHighscore(score))
        // {
        //     if (Anatidae.HighscoreManager.PlayerName == null)
        //     { // Vérifier si le joueur a saisi un pseudo ou non
        //         Anatidae.HighscoreManager.ShowHighscoreInput(score); // Lui afficher le menu de saisie de pseudo
        //     }
        //     else
        //     {
        //         // Enregistrer directement un nouveau record avec le pseudo précédemment saisi
        //         StartCoroutine(Anatidae.HighscoreManager.SetHighscore(Anatidae.HighscoreManager.PlayerName, score));
        //     }
        // }

        Debug.Log("Le joueur a été touché. Jeu arrêté !");
    }

    private void Update()
    {
        if (!isGameOver)
    {
        AudioManager.Instance.IncreasePitch("Music", 0.0005f, 2.0f); // Accélération progressive
    }


        // Vérifie si le joueur appuie sur "w" pour redémarrer
        if (Input.GetButtonDown("P1_Start"))
        {
            Debug.Log("Redémarrage via P1_Start ou touche 'ESPACE'...");
            RestartGame();
        }
    }


    public void RestartGame()
    {
        StartCoroutine(AudioManager.Instance.FadeInSound("Music", 1.0f, 3.0f)); // Fade-in sur 3 secondes
        // Recharge la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Réinitialise les variables du jeu
        isGameOver = false;
        score = 0;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
