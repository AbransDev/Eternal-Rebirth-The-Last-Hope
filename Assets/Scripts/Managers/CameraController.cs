using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Oyuncunun transformu
    public Vector3 offset;   // Kameranın oyuncuya olan mesafesi

    private void Start()
    {
        // Eğer bir offset belirlenmemişse, varsayılan olarak kameranın başlangıç pozisyonunu kullan
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    private void LateUpdate()
    {
        // Kameranın pozisyonunu oyuncunun pozisyonuna göre güncelle
        transform.position = player.position + offset;
    }
}
