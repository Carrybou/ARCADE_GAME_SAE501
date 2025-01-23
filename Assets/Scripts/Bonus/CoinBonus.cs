using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBonus : MonoBehaviour
{
    public int coinValue = 1;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Appliquer le bonus de cadence de tir
                playerController.ApplyCoinPointsBonus(coinValue);
                AudioManager.Instance.PlaySound("Coin");


                // Détruire le bonus une fois activé
                Destroy(gameObject);
            }
        }
    }
}
