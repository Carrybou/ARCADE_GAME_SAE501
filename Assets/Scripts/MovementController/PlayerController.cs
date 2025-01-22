using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FallingBall"))
        {
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
