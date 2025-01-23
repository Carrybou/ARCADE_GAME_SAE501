using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBonus : MonoBehaviour
{
    public float fireRateMultiplicator = 0.9f; // Multiplicateur pour ajuster la cadence de tir

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("FireRateBonus collision with player");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Appliquer le bonus de cadence de tir
                playerController.ApplyFireRateBonus(fireRateMultiplicator);
                AudioManager.Instance.PlaySound("Bonus");


                // Détruire le bonus une fois activé
                Destroy(gameObject);
            }
        }
    }
}
