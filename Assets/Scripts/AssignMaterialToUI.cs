using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nécessaire pour gérer TextMeshPro

public class AssignMaterialToTextUI : MonoBehaviour
{
    public Material customMaterial; // Matériau à assigner
    private TextMeshProUGUI textMeshPro; // Pour TextMeshPro
    private Text legacyText; // Pour l'ancien Text UI

    void Start()
    {
        // Vérifie si l'objet utilise TextMeshPro
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro != null && customMaterial != null)
        {
            Debug.Log("Assignation du matériau à TextMeshPro : " + customMaterial.name);
            textMeshPro.fontMaterial = customMaterial; // Applique le matériau à TextMeshPro
        }
        else
        {
            Debug.LogWarning("TextMeshPro ou matériau manquant !");
        }

        // Vérifie si l'objet utilise le Text UI classique
        legacyText = GetComponent<Text>();

        if (legacyText != null && customMaterial != null)
        {
            Debug.Log("Assignation du matériau au Text UI classique : " + customMaterial.name);
            legacyText.material = customMaterial; // Applique le matériau au Text classique
        }
        else if (legacyText == null && textMeshPro == null)
        {
            Debug.LogWarning("Aucun composant Text ou TextMeshPro trouvé !");
        }
    }
}
