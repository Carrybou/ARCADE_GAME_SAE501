using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boules : MonoBehaviour
{
    public int damage = 100;
    public int health;
    [SerializeField] TMP_Text healthText;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectiles"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);        
        }
    }
    void TakeDamage(int damage)
    {
        health -= damage; // Réduire les PV
        if (health <= 0)
        {
            Destroy(gameObject); // Détruire la boule si PV <= 0
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(5, 11);
        Debug.Log("PV de la boule : " + health);

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        
    }
}
