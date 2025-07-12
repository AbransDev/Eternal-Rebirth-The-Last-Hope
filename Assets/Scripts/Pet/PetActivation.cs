using UnityEngine;

public class PetActivation : MonoBehaviour
{
    public Transform player; // Player referansı
    public float activationDistance = 5f; // Aktivasyon mesafesi
    private PetMovement petMovement; // PetMovement script referansı
    private bool isFollowing = false; // Pet'in takip durumunu izlemek için

    public GameObject bulundu;

    void Start()
    {
        petMovement = GetComponent<PetMovement>(); // PetMovement script'ini al
        if (petMovement != null)
        {
            petMovement.enabled = false; // Başlangıçta PetMovement script'ini devre dışı bırak
        }
    }

    void Update()
    {
        if (MissionManager.Instance.currentQuestIndex == 4)
        {

            PathIndicator.Instance.target = gameObject.transform;

        }




            if (!isFollowing && Vector3.Distance(transform.position, player.position) <= activationDistance)
        {
            // Oyuncu yeterince yakınsa
            isFollowing = true; // Takip durumunu değiştir
            transform.parent = player; // Pet'i oyuncunun child'i yap
            transform.localPosition = Vector3.zero + new Vector3(0, 4, 0); // Pet'i oyuncunun kafasının üzerine yerleştir
            if (petMovement != null)
            {
                petMovement.enabled = true; // PetMovement script'ini etkinleştir
            }
            MissionManager.Instance.nextMission();
            Destroy(this); // PetActivation script'ini sil
            Time.timeScale = 0;
            bulundu.SetActive(true);

        }
    }
}
