using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreShaker : MonoBehaviour
{
    private Vector3 originalPosition;
    public float shakeDuration = 0.2f; // Durée du tremblement
    public float shakeMagnitude = 5f; // Intensité du tremblement

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
