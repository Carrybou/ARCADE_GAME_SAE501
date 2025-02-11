using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boules : MonoBehaviour
{
    public int damage = 1; // Dégâts reçus par la boule
    public int health; // Points de vie actuels
    public int healthMax; // Points de vie maximum
    [SerializeField] TMP_Text healthText; // Texte pour afficher les PV

    public string size = "large"; // "large", "medium", "small"
    public Sprite[] sprites; // Tableau de sprites pour chaque taille
    public GameObject boulePrefab; // Le prefab de la boule à instancier
    private int initialHealth; // Points de vie initiaux





    void Start()
    {
        

        // Interpréter la taille (scale) pour définir `size`
        DetermineSizeFromScale();

        // Définir les PV aléatoires (toujours entre 5 et 30)
        health = Random.Range(5, healthMax);
        initialHealth = health; // Stocker les PV initiaux


        // Appliquer un sprite aléatoire
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        Debug.Log($"Boule créée : {size} avec {health} PV");
        UpdateHealthText();
    }

    void Update()
    {
        // Mettre à jour le texte des PV
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectiles"))
        {
            Destroy(other.gameObject); // Détruire le projectile
            GameManager.Instance?.AddScore(10); // Ajouter 10 points au score
            TakeDamage(damage); // Appliquer les dégâts
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
    if (collision.gameObject.CompareTag("Ground")) // Vérifie si la boule touche le sol
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float forceX = Random.Range(-0.5f, 0.5f); // Force aléatoire vers la gauche ou la droite
            Vector2 bounceForce = new Vector2(forceX, 0.1f) * 2f; // Petite impulsion vers le haut
            rb.AddForce(bounceForce, ForceMode2D.Impulse);

        }
    }
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            HandleDestruction();
        }
    }

    void HandleDestruction()
    {
        // Vérifier la taille pour gérer les sous-boules
        if (size == "large")
        {
            SpawnSmallerBoule("medium", 2); // Crée 2 boules moyennes
            GameManager.Instance?.AddScore(20);
        }
        else if (size == "medium")
        {
            SpawnSmallerBoule("small", 2); // Crée 2 boules petites
            GameManager.Instance?.AddScore(30);
        }
        else if (size == "small")
        {
            GameManager.Instance?.AddScore(40); // Pas de sous-boules pour les petites
        }
            // Effet de tremblement de caméra
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.TriggerShake();
        }

        BonusSpawner bonusSpawner = GetComponent<BonusSpawner>();
        // Appeler le gestionnaire de bonus pour tenter de spawner un bonus
        if (bonusSpawner != null)
        {
            bonusSpawner.TrySpawnBonus(transform.position);
        }

        // Instancier le prefab SONgenerator pour jouer le son de destruction
        AudioManager.Instance.PlaySound("Explosion");



        Destroy(gameObject); // Détruire la boule actuelle
    }

    void SpawnSmallerBoule(string newSize, int count)
    {
        // Calculer les PV à donner aux enfants en fonction des PV MAXIMUM de la boule mère
        int childHealth = Mathf.Max(1, initialHealth / 2); // Chaque enfant reçoit la moitié des PV MAXIMUM de la mère
        Debug.Log($"spawning  {childHealth} health each");
        for (int i = 0; i < count; i++)
        {
            // Calculer un décalage aléatoire autour de la position actuelle
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            // Instancier une nouvelle boule avec un décalage
            GameObject newBoule = Instantiate(boulePrefab, transform.position + offset, Quaternion.identity);

            // Configurer la nouvelle boule
            Boules bouleScript = newBoule.GetComponent<Boules>();
            bouleScript.size = newSize;

            // Répartir les PV entre les boules enfants
            bouleScript.health = childHealth; // Utiliser les PV calculés
            bouleScript.healthMax = childHealth; // Mettre à jour le healthMax de l'enfant

            // Ajuster la taille et les autres propriétés en fonction de la nouvelle taille
            if (newSize == "medium")
            {
                newBoule.transform.localScale = new Vector3(0.9f, 0.9f, 1); // Taille moyenne
            }
            else if (newSize == "small")
            {
                newBoule.transform.localScale = new Vector3(0.6f, 0.6f, 1); // Taille petite
            }

            // Appliquer une légère impulsion vers le haut et une direction aléatoire (gauche ou droite)
            Rigidbody2D rb = newBoule.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Générer une impulsion aléatoire
                Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 2f)) * 2f; // Ajuster la force selon les besoins
                rb.AddForce(force, ForceMode2D.Impulse);
            }

            // Choisir un sprite aléatoire
            SpriteRenderer spriteRenderer = newBoule.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }



    void DetermineSizeFromScale()
    {
        // Récupérer le scale actuel
        float scale = transform.localScale.x;

        // Interpréter le scale pour définir la taille
        if (scale >= 1.1f) // Boules grandes
        {
            size = "large";
        }
        else if (scale >= 0.9f) // Boules moyennes
        {
            size = "medium";
        }
        else // Boules petites
        {
            size = "small";
        }
    }
}
