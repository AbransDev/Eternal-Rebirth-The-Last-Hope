using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuAnimations : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        AnimateButton(button1);
        AnimateButton(button2);
        AnimateButton(button3);
    }

    void AnimateButton(Button button)
    {
        // Basit bir ölçeklendirme animasyonu
        button.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 1f) // Animasyon süresi 1 saniye olarak ayarlandı
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
