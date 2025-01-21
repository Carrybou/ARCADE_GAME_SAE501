using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isGameOver = false;
    public int score = 0;

    private void Awake()
    {
        // Singleton : Assurez-vous qu'il n'y a qu'une seule instance de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Conserver ce GameManager entre les scènes
        }
        else
        {
            Destroy(gameObject); // Détruire les GameManagers supplémentaires
        }
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score : " + score);
    }

    public void StopGame()
    {
        isGameOver = true;

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
}
