using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject shield; // Référence au bouclier (enfant du joueur)
    private bool isShieldActive = false; // Vérifie si le bouclier est actif

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FallingBall"))
        {
            if (isShieldActive)
            {
                Destroy(collision.gameObject); // Détruit la boule au lieu du joueur
                return; // Annule la destruction du joueur
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
    }

    private IEnumerator ShieldBlinkEffect()
    {
        SpriteRenderer sr = shield.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 5; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        sr.enabled = true;
    }
}
