using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    public float collectionRange = 5f;  // Etrafındaki toplama mesafesi
    public float pullSpeed = 5f;  // Objenin oyuncuya çekilme hızı
    public float destroyDistance = 0.1f; // Objenin yok olma mesafesi
    public int missionOne, missionTwo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectNearestObject();
        }
    }

    void CollectNearestObject()
    {
        GameObject[] collectablesW = GameObject.FindGameObjectsWithTag("CollectableW");
        GameObject[] collectablesS = GameObject.FindGameObjectsWithTag("CollectableS");
        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity;

        // CollectableW objelerini kontrol et
        foreach (GameObject obj in collectablesW)
        {
            float distanceToObject = Vector3.Distance(transform.position, obj.transform.position);
            if (distanceToObject < shortestDistance && distanceToObject <= collectionRange)
            {
                shortestDistance = distanceToObject;
                nearestObject = obj;
            }
        }

        // CollectableS objelerini kontrol et
        foreach (GameObject obj in collectablesS)
        {
            float distanceToObject = Vector3.Distance(transform.position, obj.transform.position);
            if (distanceToObject < shortestDistance && distanceToObject <= collectionRange)
            {
                shortestDistance = distanceToObject;
                nearestObject = obj;
            }
        }

        if (nearestObject != null)
        {
            StartCoroutine(PullObject(nearestObject));
        }
    }

    System.Collections.IEnumerator PullObject(GameObject obj)
    {
        // Collider bileşenini devre dışı bırak
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = false;
        }

        while (Vector3.Distance(transform.position, obj.transform.position) > destroyDistance)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, pullSpeed * Time.deltaTime);
            yield return null;
        }

        // Objeyi yok etmeden önce ilgili scriptlere haber ver
        randomAgac randomAgacScript = obj.GetComponentInParent<randomAgac>();
        randomMaden randomMadenScript = obj.GetComponentInParent<randomMaden>();

        if (randomAgacScript != null)
        {
            ResourceManager.Instance.wood++;
            randomAgacScript.RemoveTree(obj);
            ResourceManager.Instance.updateStats();

            if (MissionManager.Instance.currentQuestIndex == 0)
            {
                missionOne++;
                if (missionOne == 3)
                {
                    MissionManager.Instance.nextMission();
                }
            }
        }
        else if (randomMadenScript != null)
        {
            ResourceManager.Instance.stone++;
            randomMadenScript.RemoveStone(obj);
            ResourceManager.Instance.updateStats();

            if (MissionManager.Instance.currentQuestIndex == 1)
            {
                missionTwo++;
                if (missionTwo == 3)
                {
                    MissionManager.Instance.nextMission();
                }
            }
        }

        // Objeyi yok et
        Destroy(obj);
    }
}
