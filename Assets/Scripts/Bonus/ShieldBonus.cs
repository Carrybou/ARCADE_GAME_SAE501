using UnityEngine;

public class ShieldBonus : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateShield();
            }
            Destroy(gameObject); // Détruit le bonus après ramassage
        }
    }
}
