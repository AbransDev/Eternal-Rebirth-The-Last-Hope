using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LayerControl : MonoBehaviour
{

    public LayerMask walkableLayer;
    public LayerMask unwalkableLayer;
    public float teleportDistance = 20f; // Test için mesafeyi artýrabilirsiniz
    public float yOffset = 5f; // Player objesini biraz yukarýda ýþýnlamak için
    public float safeDistance = 2f; // Walkable pozisyona minimum uzaklýk

    public List<GreenBox> greenWalkableObjects = new List<GreenBox>();

    private void Start()
    {
        greenWalkableObjects = FindObjectsOfType<GreenBox>().ToList();
    }
    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollision(other.gameObject);
    }

    private void CheckCollision(GameObject collisionObject)
    {
        // Eðer çarpýþýlan obje "unwalkable" layer'ýna sahipse
        if (unwalkableLayer == (unwalkableLayer | (1 << collisionObject.layer)))
        {
            Debug.Log("Unwalkable object hit: " + collisionObject.name);
            // En yakýn "walkable" zemin pozisyonunu bul
            Vector3 nearestWalkablePosition = FindNearestWalkablePosition();
            if (nearestWalkablePosition != Vector3.zero)
            {
                // Yeni pozisyonu kontrol et ve ýþýnla
                Vector3 newPosition = new Vector3(nearestWalkablePosition.x, nearestWalkablePosition.y + yOffset, nearestWalkablePosition.z);
                if (IsWalkablePosition(newPosition))
                {
                    transform.position = newPosition;
                    Debug.Log("Teleported to: " + transform.position);
                }
                else
                {
                    Debug.LogWarning("Found walkable position was not actually walkable: " + newPosition);
                }
            }
            else
            {
                Debug.LogWarning("No walkable position found within the teleport distance.");
            }
        }
    }

    private Vector3 FindNearestWalkablePosition()
    {
        foreach (var item in greenWalkableObjects.OrderBy(item => Vector3.Distance(transform.position, item.transform.position)))
        {
            if (IsWalkablePosition(item.transform.position))
            {
                return item.transform.position;
            }
        }
        return Vector3.zero;
    }

    private bool IsWalkablePosition(Vector3 position)
    {
        // Pozisyonun "walkable" olup olmadýðýný kontrol et
        float checkRadius = 0.5f; // Increased radius for better accuracy
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius, walkableLayer);
        return colliders.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        // Editor'de teleportDistance mesafesini görselleþtirmek için bir çember çizer
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, teleportDistance);
    }
}