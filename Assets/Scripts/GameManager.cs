using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;

    public void StopGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Le joueur a été touché. Jeu arrêté !");
    }
}
