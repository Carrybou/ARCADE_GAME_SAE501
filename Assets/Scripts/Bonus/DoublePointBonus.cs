using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointBonus : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyDoublePointsBonus();
                AudioManager.Instance.PlaySound("Bonus");
                Destroy(gameObject); // Détruit l'objet bonus après activation
            }
        }
    }
}
