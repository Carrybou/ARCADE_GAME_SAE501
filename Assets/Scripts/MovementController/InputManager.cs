using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float speed = 5f; // Vitesse de déplacement du cube

    void Update()
    {
        MovePlayer1();
        MovePlayer2();
    }

    private void MovePlayer1()
    {
        // Récupération des axes du joueur 1
        float moveHorizontal = Input.GetAxis("P1_Horizontal");
        float moveVertical = Input.GetAxis("P1_Vertical");

        // Calcul du mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * speed * Time.deltaTime;

        // Appliquer le mouvement au cube
        transform.Translate(movement, Space.World);
    }

    private void MovePlayer2()
    {
        // Récupération des axes du joueur 2
        float moveHorizontal = Input.GetAxis("P2_Horizontal");
        float moveVertical = Input.GetAxis("P2_Vertical");

        // Calcul du mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * speed * Time.deltaTime;

        // Appliquer le mouvement au cube
        transform.Translate(movement, Space.World);
    }
}