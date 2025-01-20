using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boules : MonoBehaviour
{
    public int damage = 1;
    public int health = 5;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectiles"))
        {
            Destroy(other.gameObject);
        
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
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
