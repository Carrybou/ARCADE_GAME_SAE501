using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBonus : MonoBehaviour
{
    public float fireRateMultiplicator = 0.9f; // Multiplicateur pour ajuster la cadence de tir

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController playerController = collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Appliquer le bonus de cadence de tir
                playerController.ApplyFireRateBonus(fireRateMultiplicator);

                // Détruire le bonus une fois activé
                Destroy(gameObject);
            }
        }
    }
}
