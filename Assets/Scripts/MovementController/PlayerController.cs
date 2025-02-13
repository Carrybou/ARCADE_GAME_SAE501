using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject shield; // Référence au bouclier (enfant du joueur)
    private bool isShieldActive = false; // Vérifie si le bouclier est actif
    private SpriteRenderer shieldRenderer;
    private Collider2D playerCollider;
    private Coroutine shieldCoroutine; // Pour gérer la réinitialisation du shield

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

    public void ApplyDoublePointsBonus()
    {
        if (gameManager.isDoublePointsActive)
        {
            StopCoroutine(DisableDoublePointsAfterTime()); // Réinitialise la durée si déjà actif
        }

        gameManager.isDoublePointsActive = true; // Active le x2
        Debug.Log("Bonus x2 activé !");
        StartCoroutine(DisableDoublePointsAfterTime());
    }

    private IEnumerator DisableDoublePointsAfterTime()
    {
        yield return new WaitForSeconds(5f);
        gameManager.isDoublePointsActive = false; // Désactive le x2 après 5 sec
        Debug.Log("Bonus x2 terminé !");
    }
    public void ActivateShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine); // Stoppe l'ancienne durée si le shield est déjà actif
        }

        isShieldActive = true;
        shield.SetActive(true); // Active le bouclier visuellement
        IgnoreBallCollisions(true); // Désactive les collisions avec les FallingBall

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("ShieldActivate"); // Joue un son (optionnel)
        }

        shieldCoroutine = StartCoroutine(DeactivateShieldAfterTime(5f)); // Réinitialise la durée du shield
    }

    private IEnumerator DeactivateShieldAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration - 1f); // Attendre avant de commencer à clignoter
        StartCoroutine(ShieldBlinkEffect());

        yield return new WaitForSeconds(1f); // Attendre la fin du clignotement
        shield.SetActive(false); // Désactive le bouclier
        isShieldActive = false;
        IgnoreBallCollisions(false); // Réactive les collisions avec les FallingBall
        shieldCoroutine = null; // Réinitialise la variable pour les futurs shields
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
