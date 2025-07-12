using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomAgac : MonoBehaviour
{
    public GameObject treePrefab; // Ağaç prefabı
    public int numberOfTrees = 4; // Oluşturulacak ağaç sayısı
    public Vector3 areaSize = new Vector3(10, 0, 10); // Ağaçların oluşturulacağı alanın boyutları
    public float minDistance = 4f; // Ağaçlar arasındaki minimum mesafe
    public float respawnTime = 1f; // Ağaçların yeniden doğma süresi

    private List<Vector3> treePositions = new List<Vector3>();
    private List<GameObject> currentTrees = new List<GameObject>();
    private bool isRespawning = false;

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 randomPosition;
            bool validPosition;

            do
            {
                validPosition = true;
                // Rastgele bir pozisyon oluştur
                randomPosition = new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    5f, // Y koordinatını 0.68 olarak ayarlıyoruz
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)
                );

                // Diğer ağaçlarla olan mesafeyi kontrol et
                foreach (Vector3 pos in treePositions)
                {
                    if (Vector3.Distance(randomPosition, pos) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            // Yeni bir ağaç oluştur ve pozisyonunu ayarla
            GameObject newTree = Instantiate(treePrefab, transform.position + randomPosition, Quaternion.identity, transform);
            treePositions.Add(randomPosition);
            currentTrees.Add(newTree);
        }
    }

    void Update()
    {
        if (currentTrees.Count == 0 && !isRespawning)
        {
            StartCoroutine(RespawnTrees());
        }
    }

    System.Collections.IEnumerator RespawnTrees()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnTime);
        treePositions.Clear();
        SpawnTrees();
        isRespawning = false;
    }

    public void RemoveTree(GameObject tree)
    {
        currentTrees.Remove(tree);
        Destroy(tree);
    }
}
