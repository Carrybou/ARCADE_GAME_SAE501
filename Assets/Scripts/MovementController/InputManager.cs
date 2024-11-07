using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float speed = 10f; // Vitesse de déplacement du cube
    public Transform RoueGauche;
    public Transform RoueDroite;

    void Update()
    {
        MovePlayer1();
    }

    private void MovePlayer1()
    {
        // Récupération des axes du joueur 1
        float moveHorizontal = Input.GetAxis("P1_Horizontal");

        // Calcul du mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0, 0) * speed * Time.deltaTime;

        // Appliquer le mouvement au cube
        transform.Translate(movement, Space.World);
        RoueGauche.transform.Rotate(0.0f, 0.0f, moveHorizontal*-10f, Space.Self);
        RoueDroite.transform.Rotate(0.0f, 0.0f, moveHorizontal*-10f, Space.Self);
    }


}
