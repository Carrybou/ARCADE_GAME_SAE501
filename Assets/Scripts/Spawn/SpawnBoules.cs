using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoules : MonoBehaviour
{
    public GameObject boulePrefab; // Prefab de la boule à générer
    public Sprite[] bouleSprites; // Tableau contenant les 5 sprites des boules
    public float initialSpawnInterval = 5.0f; // Intervalle initial (3 secondes)
    public float minSpawnInterval = 0.5f; // Intervalle minimum
    public float spawnAcceleration = 0.05f; // Réduction de l'intervalle à chaque spawn
    public float minScale = 0.5f; // Taille minimale des boules
    public float maxScale = 1.5f; // Taille maximale des boules

    private BoxCollider2D spawnZone; // Zone de spawn définie par le collider
    private float currentSpawnInterval; // Intervalle actuel entre les spawns

    void Start()
    {
        // Récupérer le collider attaché à cet objet
        spawnZone = GetComponent<BoxCollider2D>();
        if (spawnZone == null)
        {
            Debug.LogError("A BoxCollider2D is required for the spawn zone.");
            return;
        }

        // Initialiser l'intervalle de spawn
        currentSpawnInterval = initialSpawnInterval;

        // Démarre la boucle de spawn
        StartCoroutine(SpawnBouleCoroutine());
    }

    IEnumerator SpawnBouleCoroutine()
    {
        while (true)
        {
            // Génère une nouvelle boule
            SpawnBoule();

            // Attendre l'intervalle actuel avant de générer la suivante
            yield return new WaitForSeconds(currentSpawnInterval);

            // Réduire l'intervalle de spawn (jusqu'à un minimum)
            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= spawnAcceleration;
            }
        }
    }

    void SpawnBoule()
    {
        // Génère une position aléatoire à l'intérieur du collider
        float xPosition = Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x);
        float yPosition = Random.Range(spawnZone.bounds.min.y, spawnZone.bounds.max.y);
        Vector3 spawnPosition = new Vector3(xPosition, yPosition, 0f);

        // Instancie une nouvelle boule
        GameObject newBoule = Instantiate(boulePrefab, spawnPosition, Quaternion.identity);

        // Change la taille de la boule
        float randomScale = Random.Range(minScale, maxScale);
        newBoule.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        // Change la couleur de la boule en assignant un sprite aléatoire
        SpriteRenderer spriteRenderer = newBoule.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && bouleSprites.Length > 0)
        {
            spriteRenderer.sprite = bouleSprites[Random.Range(0, bouleSprites.Length)];
        }
    }
}
