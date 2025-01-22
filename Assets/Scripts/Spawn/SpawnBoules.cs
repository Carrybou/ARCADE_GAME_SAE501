using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoules : MonoBehaviour
{
    public GameObject boulePrefab; // Prefab de la boule à générer
    public Transform leftSpawnPoint; // Point de spawn à gauche
    public Transform rightSpawnPoint; // Point de spawn à droite
    public Sprite[] bouleSprites; // Tableau contenant les 5 sprites des boules
    public float initialSpawnInterval = 3.0f; // Intervalle initial (3 secondes)
    public float minSpawnInterval = 0.5f; // Intervalle minimum
    public float spawnAcceleration = 0.05f; // Réduction de l'intervalle à chaque spawn
    public float minScale = 0.5f; // Taille minimale des boules
    public float maxScale = 1.5f; // Taille maximale des boules
    public float forceStrength = 5.0f; // Force d'impulsion vers le centre

    private float currentSpawnInterval; // Intervalle actuel entre les spawns

    void Start()
    {
        // Initialiser l'intervalle de spawn
        currentSpawnInterval = initialSpawnInterval;

        // Démarre la boucle de spawn
        StartCoroutine(SpawnBouleCoroutine());
    }

    IEnumerator SpawnBouleCoroutine()
    {
        while (true)
        {
            // Choisit aléatoirement le point de spawn
            Transform chosenSpawnPoint = Random.value < 0.5f ? leftSpawnPoint : rightSpawnPoint;

            // Génère une nouvelle boule
            SpawnBoule(chosenSpawnPoint);

            // Attendre l'intervalle actuel avant de générer la suivante
            yield return new WaitForSeconds(currentSpawnInterval);

            // Réduire l'intervalle de spawn (jusqu'à un minimum)
            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= spawnAcceleration;
            }
        }
    }

    void SpawnBoule(Transform spawnPoint)
    {
        // Instancie une nouvelle boule à partir du point de spawn
        GameObject newBoule = Instantiate(boulePrefab, spawnPoint.position, Quaternion.identity);

        // Change la taille de la boule
        float randomScale = Random.Range(minScale, maxScale);
        newBoule.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        // Change la couleur de la boule en assignant un sprite aléatoire
        SpriteRenderer spriteRenderer = newBoule.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && bouleSprites.Length > 0)
        {
            spriteRenderer.sprite = bouleSprites[Random.Range(0, bouleSprites.Length)];
        }

        // Applique une force vers le centre du jeu
        Rigidbody2D rb = newBoule.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 directionToCenter = -spawnPoint.position.normalized; // Direction vers le centre
            rb.AddForce(directionToCenter * forceStrength, ForceMode2D.Impulse);
        }
    }
}
