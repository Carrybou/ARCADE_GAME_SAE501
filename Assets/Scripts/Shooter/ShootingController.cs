    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController2D : MonoBehaviour
{
    public GameObject projectilePrefab; // Le prefab du projectile
    public Transform firePoint;         // Le point d’ancrage pour les projectiles
    public float projectileSpeed = 10f; // Vitesse du projectile
    public float fireRate = 0.5f;       // Intervalle de tir en secondes
    public float maxProjectileHeight = 20f; // Hauteur maximale pour détruire le projectile

    private float nextFireTime = 0f;
    private Animator animator;          // Référence vers l'Animator

    void Start()
    {
        // Obtenir la référence à l'Animator attaché au personnage
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FireProjectiles();
    }

    private void FireProjectiles()
    {
        // Vérifie si c'est le moment de tirer un nouveau projectile
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            // Crée une instance du projectile à la position du firePoint
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Configure la direction et la vitesse en 2D
            Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; 
            rb.velocity = Vector2.up * projectileSpeed;

            // Détruit le projectile lorsqu'il atteint la hauteur maximale
            Destroy(projectile, maxProjectileHeight / projectileSpeed);

            // Déclenche l'animation de tir
            animator.SetTrigger("ShootTrigger");
        }
    }
}
