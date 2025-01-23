using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 2.0f; // Vitesse du mouvement
    public float distance = 5.0f; // Distance du mouvement à gauche et à droite
    public Transform RoueGauche;
    public Transform RoueDroite;

    private Vector3 startPos; // Position de départ

    void Start()
    {
        // Stocker la position de départ du cube
        startPos = transform.position;
    }

    void Update()
    {
        // Calculer la nouvelle position en oscillant de gauche à droite
        float x = startPos.x + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(x, startPos.y, startPos.z);

        // Calculer une rotation des roues basée sur la vitesse de déplacement
        float rotationSpeed = speed * Mathf.Cos(Time.time * speed) * distance; // Cosinus pour la dérivée de Sinus
        RoueGauche.Rotate(0.0f, 0.0f, rotationSpeed, Space.Self);
        RoueDroite.Rotate(0.0f, 0.0f, rotationSpeed, Space.Self);
    }
}
