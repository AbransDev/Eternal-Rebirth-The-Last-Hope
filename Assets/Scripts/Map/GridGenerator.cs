using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Küp modelinizin prefab'ı
    public int gridSize = 100; // Grid boyutu
    public float spacing = 1.1f; // Küpler arası mesafe

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Script'in bulunduğu obje altında "Zemin" adında bir GameObject oluştur
        GameObject zemin = new GameObject("BadSide");
        zemin.transform.parent = transform;

        // Orta kısmı belirlemek için koordinatlar
        int center = gridSize / 2;
        int halfGapSize = 5 / 2;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // 5x5 boşluk bırakılacak alanı kontrol et
                if (x >= center - halfGapSize && x <= center + halfGapSize && y >= center - halfGapSize && y <= center + halfGapSize)
                {
                    continue; // Bu alanı atla
                }

                Vector3 position = new Vector3(x * spacing, 0, y * spacing);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.transform.parent = zemin.transform; // Küpü "Zemin" GameObject'inin altına yerleştir
                cube.tag = "BadBox"; // Küpe "BadBox" tag'ini ver
            }
        }
    }
}
