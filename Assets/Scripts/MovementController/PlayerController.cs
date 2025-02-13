using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject shield; // Référence au bouclier (enfant du joueur)
    private bool isShieldActive = false;
    private SpriteRenderer shieldRenderer;
    private Collider2D playerCollider;
    private Coroutine shieldCoroutine; // Pour gérer la réinitialisation du shield

    private UiManager uiManager; // Référence au UI Manager

    void Start()
    {
        shieldRenderer = shield.GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        uiManager = FindObjectOfType<UiManager>(); // Trouve le UI Manager dans la scène
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FallingBall"))
        {
            if (isShieldActive)
            {
                return; // La boule traverse le joueur sans aucun effet
            }

            Destroy(gameObject);
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

        gameManager.isDoublePointsActive = true; // Active le bonus x2
        Debug.Log("Bonus x2 activé !");

        if (uiManager != null)
        {
            uiManager.ShowDoublePointsText(); // Affiche "DOUBLE POINTS"
        }

        StartCoroutine(DisableDoublePointsAfterTime());
    }

    private IEnumerator DisableDoublePointsAfterTime()
    {
        yield return new WaitForSeconds(4f); // Attends 4 secondes avant de commencer à clignoter

        if (uiManager != null)
        {
            StartCoroutine(uiManager.BlinkDoublePointsText()); // Lance le clignotement
        }

        yield return new WaitForSeconds(1f); // Dernière seconde avant la fin

        gameManager.isDoublePointsActive = false;
        Debug.Log("Bonus x2 terminé !");

        if (uiManager != null)
        {
            uiManager.HideDoublePointsText(); // Cache le texte
        }
    }

    public void ActivateShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        isShieldActive = true;
        shield.SetActive(true);
        IgnoreBallCollisions(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("ShieldActivate");
        }

        shieldCoroutine = StartCoroutine(DeactivateShieldAfterTime(5f));
    }

    private IEnumerator DeactivateShieldAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration - 1f);
        StartCoroutine(ShieldBlinkEffect());

        yield return new WaitForSeconds(1f);
        shield.SetActive(false);
        isShieldActive = false;
        IgnoreBallCollisions(false);
        shieldCoroutine = null;
    }

    private IEnumerator ShieldBlinkEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            shieldRenderer.enabled = !shieldRenderer.enabled;
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
