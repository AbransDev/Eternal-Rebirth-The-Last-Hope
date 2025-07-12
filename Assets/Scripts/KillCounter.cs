using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance;
    private void Awake()
    {
        Instance = this;
    }
    public int killCount;

    private void Update()
    {
        if (MissionManager.Instance.currentQuestIndex == 3 && killCount >=3)
        {
            MissionManager.Instance.nextMission();
        }


    }

}
