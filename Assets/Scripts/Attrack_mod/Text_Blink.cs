using UnityEngine;
using TMPro;  // Inclure le namespace TMP

public class TextBlink : MonoBehaviour
{
    public float blinkSpeed = 1.0f;  // Vitesse du clignotement (en secondes)
    private TMP_Text textComponent;   // Composant TextMesh Pro du UI

    void Start()
    {
        // Récupère le composant TMP_Text du GameObject auquel le script est attaché
        textComponent = GetComponent<TMP_Text>();
        
        if (textComponent == null) {
            Debug.LogError("Aucun composant TMP_Text trouvé sur ce GameObject !");
        }
    }

    void Update()
    {
        // Vérifie que le composant TMP_Text est bien récupéré
        if (textComponent != null)
        {
            // Change l'alpha (transparence) du texte entre 0 (transparent) et 1 (opaque)
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            Color color = textComponent.color;
            color.a = alpha;  // Applique l'alpha modifié
            textComponent.color = color;  // Applique la couleur mise à jour
        }
    }
}
