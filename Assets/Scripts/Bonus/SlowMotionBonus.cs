using System.Collections;
using UnityEngine;

public class SlowMotionBonus : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplySlowMotionBonus();
                AudioManager.Instance.PlaySound("Bonus");

                Destroy(gameObject); // ✅ Détruit le bonus après activation
            }
        }
    }
}
