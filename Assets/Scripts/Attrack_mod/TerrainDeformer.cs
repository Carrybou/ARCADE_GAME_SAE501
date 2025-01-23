using UnityEngine;

public class MountainAnimator : MonoBehaviour
{
    public Terrain terrain; // Ton terrain
    public float waveSpeed = 0.5f; // Vitesse des oscillations
    public float waveHeight = 0.1f; // Amplitude des oscillations
    public float waveFrequency = 1f; // Fréquence des oscillations
    public float minMountainHeight = 0.3f; // Hauteur minimum pour être considéré comme une montagne

    private float[,] originalHeights; // Sauvegarde des hauteurs originales
    private TerrainData terrainData;

    void Start()
    {
        // Récupère les données du terrain
        terrainData = terrain.terrainData;

        // Sauvegarde les hauteurs originales
        originalHeights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
    }

    void Update()
    {
        AnimateMountains();
    }

    void AnimateMountains()
    {
        // Récupère la résolution de la heightmap
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        // Crée un tableau pour les nouvelles hauteurs
        float[,] newHeights = new float[heightmapWidth, heightmapHeight];

        // Parcourt chaque point de la heightmap
        for (int x = 0; x < heightmapWidth; x++)
        {
            for (int y = 0; y < heightmapHeight; y++)
            {
                // Applique une animation uniquement si la hauteur est supérieure à minMountainHeight
                if (originalHeights[x, y] > minMountainHeight)
                {
                    float wave = Mathf.Sin((x + Time.time * waveSpeed) * waveFrequency) 
                               * Mathf.Sin((y + Time.time * waveSpeed) * waveFrequency);
                    newHeights[x, y] = originalHeights[x, y] + wave * waveHeight;
                }
                else
                {
                    // Conserve les hauteurs originales pour les zones plates
                    newHeights[x, y] = originalHeights[x, y];
                }
            }
        }

        // Applique les nouvelles hauteurs au terrain
        terrainData.SetHeights(0, 0, newHeights);
    }

    void OnDisable()
    {
        // Réinitialise les hauteurs du terrain lorsque le jeu s'arrête
        if (terrainData != null && originalHeights != null)
        {
            terrainData.SetHeights(0, 0, originalHeights);
        }
    }
}
