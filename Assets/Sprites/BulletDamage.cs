using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damageAmount = 10; // Verilecek hasar miktarý

    private void OnCollisionEnter(Collision collision)
    {
        // Çarptýðý objenin tag'i "Enemy" mi?
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // EnemyHealth scriptini al
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // TakeDamage fonksiyonunu çaðýr
                enemyHealth.TakeDamage(damageAmount);
            }

            // Prefabý yok et
            Destroy(gameObject);
        }
    }
}
