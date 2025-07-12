using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneCollector : MonoBehaviour
{
    public float detectionRadius = 50f;
    public float collectionTime = 3f;
    private NavMeshAgent agent;
    private GameObject currentTarget;
    private Vector3 spawnPosition;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPosition = new Vector3(transform.position.x, 0, transform.position.z); // NPC'nin baþlangýç pozisyonunu kaydet
        StartCoroutine(CollectRoutine());
    }

    IEnumerator CollectRoutine()
    {
        while (true)
        {
            FindNewTarget();

            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.transform.position);
                animator.SetBool("isRunning", true); // Run animasyonuna geç

                yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

                animator.SetBool("isRunning", false); // Idle animasyonuna geç
                yield return new WaitForSeconds(collectionTime);

                CollectTarget();
            }
            yield return null;
        }
    }

    void FindNewTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, detectionRadius);
        float minDistance = Mathf.Infinity;
        GameObject nearestCollectable = null;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("CollectableS"))
            {
                Vector3 targetPosition = new Vector3(collider.transform.position.x, 0, collider.transform.position.z);
                float distance = Vector3.Distance(spawnPosition, targetPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCollectable = collider.gameObject;
                }
            }
        }

        currentTarget = nearestCollectable;
    }

    void CollectTarget()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }
    }

    void Update()
    {
        // NPC'nin çemberin dýþýna çýkmasýný engelle
        Vector3 npcPosition = new Vector3(transform.position.x, 0, transform.position.z);
        if (Vector3.Distance(spawnPosition, npcPosition) > detectionRadius)
        {
            agent.SetDestination(spawnPosition);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(spawnPosition.x, 0, spawnPosition.z), detectionRadius);
    }
}