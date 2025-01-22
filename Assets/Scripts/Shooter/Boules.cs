using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boules : MonoBehaviour
{
    public int damage = 1;
    public int health;
    [SerializeField] TMP_Text healthText;

    public string size = "large"; // "large", "medium", "small"
    public Sprite[] sprites; // Tableau de sprites pour chaque taille
    public GameObject boulePrefab; // Le prefab de la boule à instancier

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectiles"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(10);
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage; // Réduire les PV
        if (health <= 0)
        {
            if (size == "large")
            {
                SpawnSmallerBoule("medium", 2); // Crée 2 boules moyennes
                GameManager.Instance.AddScore(20);
            }
            else if (size == "medium")
            {
                SpawnSmallerBoule("small", 2); // Crée 2 boules petites
                GameManager.Instance.AddScore(30);
            }
            else if (size == "small")
            {
                GameManager.Instance.AddScore(40);
            }
            BonusSpawner bonusSpawner = GetComponent<BonusSpawner>();
            // Appeler le gestionnaire de bonus pour tenter de spawner un bonus
            if (bonusSpawner != null)
            {
                bonusSpawner.TrySpawnBonus(transform.position);
            }

            Destroy(gameObject); // Détruire la boule actuelle
        }
    }

    void SpawnSmallerBoule(string newSize, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Calculer un décalage aléatoire autour de la position actuelle
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            // Instancier une nouvelle boule avec un décalage
            GameObject newBoule = Instantiate(boulePrefab, transform.position + offset, Quaternion.identity);

            // Configurer la nouvelle boule
            Boules bouleScript = newBoule.GetComponent<Boules>();
            bouleScript.size = newSize;

            // Ajuster la taille et les PV en fonction de la nouvelle taille
            if (newSize == "medium")
            {
                bouleScript.health = Random.Range(3, 6); // PV pour une boule moyenne
                newBoule.transform.localScale = new Vector3(0.9f, 0.9f, 1); // Taille moyenne
            }
            else if (newSize == "small")
            {
                bouleScript.health = Random.Range(1, 3); // PV pour une boule petite
                newBoule.transform.localScale = new Vector3(0.6f, 0.6f, 1); // Taille petite
            }

            // Choisir un sprite aléatoire
            SpriteRenderer spriteRenderer = newBoule.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Initialiser les PV en fonction de la taille
        if (size == "large")
        {
            health = Random.Range(5, 11);
        }
        else if (size == "medium")
        {
            health = Random.Range(3, 6);
        }
        else if (size == "small")
        {
            health = Random.Range(1, 3);
        }

        // Appliquer un sprite aléatoire au départ
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        Debug.Log("PV de la boule (" + size + ") : " + health);
    }

    // Update is called once per frame
    void Update()
    {
        // Mettre à jour le texte des PV
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }
}
