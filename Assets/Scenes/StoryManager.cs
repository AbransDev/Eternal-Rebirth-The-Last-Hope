using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChildSwitcher : MonoBehaviour
{
    public GameObject[] children;
    public string nextSceneName;

    
    private void Start()
    {
        if (children.Length < 3)
        {
            Debug.LogError("Çocuk nesnelerden en az üç tane olmalı.");
            return;
        }

        // Başlangıçta sadece ilk çocuğu aktif yap
        ActivateChild(0);
        // İlk 10 saniyelik beklemeyi başlat
        StartCoroutine(SwitchChildren());
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {

           LoadNextScene();
            
        }
    }

    private IEnumerator SwitchChildren()
    {
        yield return new WaitForSeconds(10f);
        ActivateChild(1);

        yield return new WaitForSeconds(10f);
        ActivateChild(2);

        yield return new WaitForSeconds(10f);
        LoadNextScene();
    }

    private void ActivateChild(int index)
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(i == index);
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

