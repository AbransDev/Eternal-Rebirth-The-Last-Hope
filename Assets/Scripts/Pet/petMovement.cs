using UnityEngine;

public class PetMovement : MonoBehaviour
{
    public Transform player; // Player referansı
    public Vector3 offset = new Vector3(0, 2, 0); // Player'ın kafasının üstünde başlangıç konumu
    public float followSpeed = 2.0f; // Player'ı takip etme hızı
    public float verticalSpeed = 2.0f; // Yukarı-aşağı hareket hızı
    public float verticalAmplitude = 0.5f; // Yukarı-aşağı hareket genliği

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Player'ı takip et
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Yukarı-aşağı hareket
        Vector3 verticalMovement = initialPosition;
        verticalMovement.y += Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
        transform.localPosition = verticalMovement;
    }
}
