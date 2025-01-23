using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttractMode : MonoBehaviour
{
    void Update()
    {
        // Vérifie si une touche a été appuyée pour quitter le mode d'attraction
        if (Input.anyKeyDown)
        {
            LoadGameScene();
        }
    }

    void LoadGameScene()
    {
        // Remplace "NomDeLaSceneDeJeu" par le nom de la scène de ton jeu principal
        SceneManager.LoadScene("Casse_Les_Boules");
    }
}
