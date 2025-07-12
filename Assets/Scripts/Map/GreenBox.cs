using UnityEngine;

public class GreenBox : MonoBehaviour
{
    // "BadBox" objelerinin yok edilebileceği mesafe
    float destroyDistance = 16f;


    public void levelUp()
    {
        
            // Tüm "BadBox" taglı objeleri bul
            GameObject[] badBoxes = GameObject.FindGameObjectsWithTag("BadBox");

            // Her bir "BadBox" objesi için
            foreach (GameObject badBox in badBoxes)
            {
                // Mevcut obje ile "BadBox" objesi arasındaki mesafeyi hesapla
                float distance = Vector3.Distance(transform.position, badBox.transform.position);

                // Eğer mesafe belirtilen yok etme mesafesinden azsa
                if (distance <= destroyDistance)
                {
                    // "BadBox" objesini yok et
                    Destroy(badBox);
                }
            }

            // Tüm "GreenAFK" taglı objeleri bul
            GameObject[] greenAFKBoxes = GameObject.FindGameObjectsWithTag("GreenAFK");

            // Her bir "GreenAFK" objesi için
            foreach (GameObject greenAFKBox in greenAFKBoxes)
            {
                // Mevcut obje ile "GreenAFK" objesi arasındaki mesafeyi hesapla
                float distance = Vector3.Distance(transform.position, greenAFKBox.transform.position);

                // Eğer mesafe belirtilen yok etme mesafesinden azsa
                if (distance <= destroyDistance)
                {
                    // "GreenAFK" objesinin tagını "GreenBox" olarak değiştir
                    greenAFKBox.tag = "GreenBox";

                    // "GreenBox" objesine "DestroyBadBox" scriptini ekle
                    if (greenAFKBox.GetComponent<GreenBox>() == null)
                    {
                        greenAFKBox.AddComponent<GreenBox>();
                    }
                }
            }

        Destroy(this);
    }
}
