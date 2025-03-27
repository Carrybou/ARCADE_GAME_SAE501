using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bonus
{
    public GameObject prefab; // Le prefab du bonus
    public float spawnChance; // La chance d'apparition de ce bonus (exprimée en pourcentage ou en probabilité)
}

public class BonusSpawner : MonoBehaviour
{
    public Bonus[] bonuses; // Tableau des bonus avec leur probabilité respective
    public float globalSpawnChance = 0.3f; // Probabilité globale de spawn d'un bonus

    public void TrySpawnBonus(Vector2 position)
    {
        // Générer un nombre aléatoire pour la probabilité globale
        if (Random.value <= globalSpawnChance)
        {
            // Générer un nombre aléatoire entre 0 et la somme totale des probabilités
            float totalChance = 0f;
            foreach (var bonus in bonuses)
            {
                totalChance += bonus.spawnChance;
            }

            float randomValue = Random.Range(0f, totalChance);
            float cumulativeChance = 0f;

            // Parcourir les bonus et déterminer lequel doit être spawné
            foreach (var bonus in bonuses)
            {
                cumulativeChance += bonus.spawnChance;
                if (randomValue <= cumulativeChance)
                {
                    Debug.Log($"Spawning bonus: {bonus.prefab.name}");
                    Instantiate(bonus.prefab, position, Quaternion.identity);
                    return;
                }
            }
        }
    }
}
