using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    public GameObject tab1, tab2;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject Player, deadVFX;

    public void okay1()
    {
        tab1.SetActive(false);
        Time.timeScale = 1;
    }

    public void okay2()
    {
        tab2.SetActive(false);
        Time.timeScale = 1;
    }
}
