using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuKuyusu : MonoBehaviour
{
    private bool isOpen = false;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        // Debugging purpose
        Debug.Log(isOpen);

        // Check if the current mission index is 2 and set the path indicator target
        if (MissionManager.Instance.currentQuestIndex == 2)
        {
            PathIndicator.Instance.target = transform;
        }

        // Measure the distance between the player and the well
        if (Vector3.Distance(playerTransform.position, transform.position) <= 7f)
        {
            isOpen = true;
        }

        // If the well is open, perform the completion actions
        if (isOpen)
        {
            PathIndicator.Instance.target = null;
            MissionManager.Instance.nextMission();
            ResourceManager.Instance.waterGelir += 3;
            Destroy(this);
        }

    }
}
