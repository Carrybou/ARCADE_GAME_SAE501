using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject shield; // Référence au bouclier (enfant du joueur)
    private bool isShieldActive = false; // Vérifie si le bouclier est actif
    private SpriteRenderer shieldRenderer;
    private Collider2D playerCollider;

    void Start()
    {
        shieldRenderer = shield.GetComponent<SpriteRenderer>(); // Récupère le SpriteRenderer du bouclier
        playerCollider = GetComponent<Collider2D>(); // Récupère le collider du joueur
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FallingBall"))
        {
            if (isShieldActive)
            {
                return; // La boule traverse le joueur sans aucun effet
            }

            Destroy(gameObject); // Détruit le joueur si pas de bouclier
            gameManager.StopGame();
        }
    }

    public void ApplyFireRateBonus(float fireRateMultiplicator)
    {
        ShootingController2D shootingController = GetComponent<ShootingController2D>();
        if (shootingController != null)
        {
            shootingController.fireRate *= fireRateMultiplicator;
        }
    }

    public void ApplyCoinPointsBonus(int coinValue)
    {
        gameManager.AddScore(coinValue);
    }

    public void ActivateShield()
    {
        if (isShieldActive) return; // Empêche d'activer le bouclier plusieurs fois

        isShieldActive = true;
        shield.SetActive(true); // Active le bouclier visuellement
        IgnoreBallCollisions(true); // Désactive les collisions avec les FallingBall

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("ShieldActivate"); // Joue un son (optionnel)
        }

        StartCoroutine(DeactivateShieldAfterTime(5f)); // Lance la désactivation après 5 sec
    }

    private IEnumerator DeactivateShieldAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration - 1f); // Attendre avant de commencer à clignoter
        StartCoroutine(ShieldBlinkEffect());

        yield return new WaitForSeconds(1f); // Attendre la fin du clignotement
        shield.SetActive(false); // Désactive le bouclier
        isShieldActive = false;
        IgnoreBallCollisions(false); // Réactive les collisions avec les FallingBall
    }

    private IEnumerator ShieldBlinkEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            shieldRenderer.enabled = !shieldRenderer.enabled; // Clignotement avant expiration
            yield return new WaitForSeconds(0.2f);
        }
        shieldRenderer.enabled = true;
    }

    private void IgnoreBallCollisions(bool ignore)
    {
        GameObject[] fallingBalls = GameObject.FindGameObjectsWithTag("FallingBall");

        foreach (GameObject ball in fallingBalls)
        {
            Collider2D ballCollider = ball.GetComponent<Collider2D>();
            if (ballCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, ballCollider, ignore);
            }
        }
    }
}
