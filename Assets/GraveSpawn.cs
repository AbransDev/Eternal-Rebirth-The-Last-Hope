using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraveSpawn : MonoBehaviour
{
    public GameObject tab2;
    public GameObject[] npcPrefabs; // NPC prefab array
    public int initialWoodCost = 20;
    public int initialStoneCost = 20;
    public int woodIncrement = 10;
    public int stoneIncrement = 10;
    public int maxNPCs = 5;

    private int currentWoodCost;
    private int currentStoneCost;
    private int npcCount = 0;

    public TMP_Text woodCostText;
    public TMP_Text stoneCostText;
    private ResourceManager resourceManager;

    public Transform npcSpawnPosition; // NPC'nin do�aca�� pozisyon
    public Transform playerTransform; // Oyuncunun transformu
    public float activationDistance = 5f; // Oyuncunun mezarl��a ne kadar yak�n olmas� gerekti�i

    void Start()
    {
        currentWoodCost = initialWoodCost;
        currentStoneCost = initialStoneCost;
        resourceManager = ResourceManager.Instance;
        UpdateCostUI();
        SetTextActive(false); // Ba�lang��ta textleri gizle
    }

    void Update()
    {
        if (MissionManager.Instance.currentQuestIndex == 5)

        {
            PathIndicator.Instance.target = gameObject.transform;

        }


            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= activationDistance)
        {
            SetTextActive(true); // Oyuncu mezara yak�nsa textleri g�ster

            if (Input.GetKeyDown(KeyCode.F)) // 'F' tu�una bas�lma kontrol�
            {
                TryAwakenNPC();
            }
        }
        else
        {
            SetTextActive(false); // Oyuncu mezara uzaksa textleri gizle
        }
    }

    void SetTextActive(bool isActive)
    {
        woodCostText.gameObject.SetActive(isActive);
        stoneCostText.gameObject.SetActive(isActive);
    }

    void UpdateCostUI()
    {
        woodCostText.text = "Wood: " + currentWoodCost;
        stoneCostText.text = "Stone: " + currentStoneCost;
    }

    public void TryAwakenNPC()
    {
        if (npcCount < maxNPCs && resourceManager.wood >= currentWoodCost && resourceManager.stone >= currentStoneCost)
        {
            resourceManager.wood -= currentWoodCost;
            resourceManager.stone -= currentStoneCost;
            npcCount++;
            AwakenNPC();

            currentWoodCost += woodIncrement;
            currentStoneCost += stoneIncrement;
            UpdateCostUI();
            MissionManager.Instance.nextMission();
            PathIndicator.Instance.target = null;
            Time.timeScale = 0;
            tab2.SetActive(true);
        }
    }

    void AwakenNPC()
    {
        // Rastgele bir NPC prefab se� ve belirlenen pozisyonda instantiate et
        int randomIndex = Random.Range(0, npcPrefabs.Length);
        GameObject npc = Instantiate(npcPrefabs[randomIndex], npcSpawnPosition.position, npcSpawnPosition.rotation);
        ResourceManager.Instance.population++;
        Debug.Log("NPC Uyand�r�ld�: " + npc.name);
    }
}