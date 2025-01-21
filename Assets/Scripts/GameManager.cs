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
        Time.timeScale = 0f;
        Debug.Log("Le joueur a été touché. Jeu arrêté !");
    }
}
