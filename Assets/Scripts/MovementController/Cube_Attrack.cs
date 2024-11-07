using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 2.0f; // Vitesse du mouvement
    public float distance = 5.0f; // Distance du mouvement à gauche et à droite

    private Vector3 startPos; // Position de départ

    void Start()
    {
        // Stocker la position de départ du cube
        startPos = transform.position;
    }

    void Update()
    {
        // Calculer la nouvelle position en utilisant Mathf.Sin pour osciller de gauche à droite
        float x = startPos.x + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(x, startPos.y, startPos.z);
    }
}

