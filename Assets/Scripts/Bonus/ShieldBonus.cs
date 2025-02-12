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
                AudioManager.Instance.PlaySound("Bonus");
                AudioManager.Instance.PlaySound("Shield");
            }
            Destroy(gameObject); // Détruit le bonus après ramassage
        }
    }
}
