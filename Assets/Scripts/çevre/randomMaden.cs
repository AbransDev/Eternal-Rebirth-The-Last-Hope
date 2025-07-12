using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMaden : MonoBehaviour
{
    public GameObject stonePrefab; // Taş prefabı
    public int numberOfStones = 4; // Oluşturulacak taş sayısı
    public Vector3 areaSize = new Vector3(9, 0, 9); // Taşların oluşturulacağı alanın boyutları
    public float minDistance = 4f; // Taşlar arasındaki minimum mesafe
    public float respawnTime = 30f; // Taşların yeniden doğma süresi

    private List<Vector3> stonePositions = new List<Vector3>();
    private List<GameObject> currentStones = new List<GameObject>();
    private bool isRespawning = false;

    void Start()
    {
        SpawnStones();
    }

    void SpawnStones()
    {
        for (int i = 0; i < numberOfStones; i++)
        {
            Vector3 randomPosition;
            bool validPosition;

            do
            {
                validPosition = true;
                // Rastgele bir pozisyon oluştur
                randomPosition = new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    5f, // Y koordinatını ayarlıyoruz
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)
                );

                // Diğer taşlarla olan mesafeyi kontrol et
                foreach (Vector3 pos in stonePositions)
                {
                    if (Vector3.Distance(randomPosition, pos) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            // Yeni bir taş oluştur ve pozisyonunu ayarla
            GameObject newStone = Instantiate(stonePrefab, transform.position + randomPosition, Quaternion.identity, transform);
            stonePositions.Add(randomPosition);
            currentStones.Add(newStone);
        }
    }

    void Update()
    {
        if (currentStones.Count == 0 && !isRespawning)
        {
            StartCoroutine(RespawnStones());
        }
    }

    System.Collections.IEnumerator RespawnStones()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnTime);
        stonePositions.Clear();
        SpawnStones();
        isRespawning = false;
    }

    public void RemoveStone(GameObject stone)
    {
        currentStones.Remove(stone);
        Destroy(stone);
    }
}
