using UnityEngine;

public class JoystickAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Amplitude maximale de la rotation en degrés.")]
    public float rotationAngle = 10f; // Amplitude de rotation (en degrés)

    [Tooltip("Vitesse de l'animation.")]
    public float rotationSpeed = 2f; // Vitesse d'oscillation (en cycles/seconde)

    void Update()
    {
        // Calculer l'angle de rotation basé sur une oscillation sinusoïdale
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;

        // Appliquer la rotation locale autour du pivot défini
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
