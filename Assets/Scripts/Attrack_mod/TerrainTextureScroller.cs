using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeedY = 0.05f;  // Vitesse de défilement vertical (plus petit = plus lent)

    private Renderer rend;

    void Start()
    {
        // Récupérer le composant Renderer du quad (l'objet auquel le matériau est appliqué)
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Défilement vertical : augmente l'offset Y au fil du temps pour faire défiler l'image de bas en haut
        float offsetY = Time.time * scrollSpeedY;  // Défilement vertical

        // Appliquer le décalage à la texture (seulement sur Y)
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offsetY));
    }
}

