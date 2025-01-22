using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public GameObject[] bonusPrefabs; // Tableau contenant les prefabs des différents bonus
    public float spawnChance = 0.3f; // Probabilité qu'un bonus spawn (entre 0 et 1)

    public void TrySpawnBonus(Vector2 position)
    {
        // Générer un nombre aléatoire entre 0 et 1
        if (Random.value <= spawnChance)
        {
            // Choisir un bonus aléatoire dans le tableau
            int randomIndex = Random.Range(0, bonusPrefabs.Length);
            GameObject bonusPrefab = bonusPrefabs[randomIndex];

            // Instancier le bonus à la position donnée
            Instantiate(bonusPrefab, position, Quaternion.identity);
        }
    }
}
