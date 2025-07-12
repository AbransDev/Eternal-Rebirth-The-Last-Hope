using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{


    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // D��man �lme i�lemleri (�rn. animasyon oynatma, yok etme)
        Debug.Log("Enemy Died");
        ResourceManager.Instance.food += 2;
        if (MissionManager.Instance.currentQuestIndex == 3)
        {

            KillCounter.Instance.killCount++;
        }
        InstantiateNewObject();
        Destroy(gameObject);
    }

    void InstantiateNewObject()
    {

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Instantiate(GameManager.Instance.deadVFX, position, rotation);
    }


}