using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class Boules : MonoBehaviour
{
    public int damage = 1;
    public int health;
    public int healthMax;
    [SerializeField] TMP_Text healthText;

    public string size = "large";
    public Sprite[] sprites;
    public GameObject boulePrefab;
    public GameObject Explosionprefab; // Le prefab du bonus

    private int initialHealth;
    public ScoreShaker scoreShaker;
    private static int layerOrderCounter = 0;
    private SortingGroup sortingGroup;
    private Canvas canvas;
    private Rigidbody2D rb;
    private float baseSpeed = 2f;
    private float baseGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseGravity = rb.gravityScale; // ✅ Sauvegarde la gravité originale

        ApplySpeed(); // ✅ Applique la vitesse au départ

        DetermineSizeFromScale();

        health = Random.Range(5, healthMax);
        initialHealth = health;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        sortingGroup = GetComponent<SortingGroup>();
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = layerOrderCounter;
            layerOrderCounter += 2;
        }

        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = sortingGroup != null ? sortingGroup.sortingOrder + 1 : 1;
        }

        scoreShaker = FindObjectOfType<ScoreShaker>();
        UpdateHealthText();
    }

    void Update()
    {
        ApplySpeed(); // ✅ Assure que le ralentissement est appliqué dynamiquement
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
            Destroy(other.gameObject);
            GameManager.Instance?.AddScore(10);
            TakeDamage(damage);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (rb != null)
            {
                float forceX = Random.Range(-0.5f, 0.5f);
                Vector2 bounceForce = new Vector2(forceX, 0.1f) * 2f;
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

        if (size == "large")
        {
            SpawnSmallerBoule("medium", 2);
            GameManager.Instance?.AddScore(20);
        }
        else if (size == "medium")
        {
            SpawnSmallerBoule("small", 2);
            GameManager.Instance?.AddScore(30);
        }
        else if (size == "small")
        {
            GameManager.Instance?.AddScore(40);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.TriggerShake();
        }

        BonusSpawner bonusSpawner = GetComponent<BonusSpawner>();
        if (bonusSpawner != null)
        {
            bonusSpawner.TrySpawnBonus(transform.position);
        }

        AudioManager.Instance.PlaySound("Explosion");

        if (scoreShaker != null)
        {
            scoreShaker.Shake();
        }
        
        GameObject explosionInstance = Instantiate(Explosionprefab, transform.position, Quaternion.identity);
        Destroy(explosionInstance, 0.4f);



        Destroy(gameObject);
    }

    void SpawnSmallerBoule(string newSize, int count)
    {
        int childHealth = Mathf.Max(1, initialHealth / 2);
        Debug.Log($"spawning  {childHealth} health each");
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject newBoule = Instantiate(boulePrefab, transform.position + offset, Quaternion.identity);

            Boules bouleScript = newBoule.GetComponent<Boules>();
            bouleScript.size = newSize;
            bouleScript.health = childHealth;
            bouleScript.healthMax = childHealth;

            if (newSize == "medium")
            {
                newBoule.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            }
            else if (newSize == "small")
            {
                newBoule.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }

            Rigidbody2D rb = newBoule.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 1.5f));
                rb.AddForce(force, ForceMode2D.Impulse);
            }

            SpriteRenderer spriteRenderer = newBoule.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }

    void DetermineSizeFromScale()
    {
        float scale = transform.localScale.x;

        if (scale >= 1.1f)
        {
            size = "large";
        }
        else if (scale >= 0.9f)
        {
            size = "medium";
        }
        else
        {
            size = "small";
        }
    }

    void ApplySpeed()
    {
        if (rb != null)
        {
            float multiplier = GameManager.Instance.GetSlowMotionMultiplier();

            // ✅ Ralentir à fond si slow motion activé
            if (GameManager.Instance.isSlowMotionActive)
            {
                rb.velocity *= multiplier;
                rb.gravityScale = baseGravity * multiplier; // ✅ Ralentir aussi la chute pour éviter qu'elles roulent
            }
            else
            {
                rb.gravityScale = baseGravity;
            }
        }
    }
}
