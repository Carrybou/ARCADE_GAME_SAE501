using UnityEngine;

public class JoystickMouvement : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Vitesse de rotation
    public float rotationAngle = 15.0f; // Angle maximal de rotation

    private float currentAngle = 0.0f;
    private bool rotatingRight = true;

    void Update()
    {
        // Calculer l'angle de rotation
        if (rotatingRight)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            if (currentAngle >= rotationAngle)
            {
                currentAngle = rotationAngle;
                rotatingRight = false;
            }
        }
        else
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
            if (currentAngle <= -rotationAngle)
            {
                currentAngle = -rotationAngle;
                rotatingRight = true;
            }
        }

        // Appliquer la rotation au GameObject parent
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
