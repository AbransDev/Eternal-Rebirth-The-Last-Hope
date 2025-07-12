using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtons : MonoBehaviour
{
    public GameObject tt1,tt2, play, next;

    public void next_btm()
    {
      
        tt1.SetActive(false);
        tt2.SetActive(true);
        play.SetActive(true);
        Destroy(next);
        

    }
}
