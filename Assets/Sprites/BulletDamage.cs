using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damageAmount = 10; // Verilecek hasar miktar�

    private void OnCollisionEnter(Collision collision)
    {
        // �arpt��� objenin tag'i "Enemy" mi?
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // EnemyHealth scriptini al
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // TakeDamage fonksiyonunu �a��r
                enemyHealth.TakeDamage(damageAmount);
            }

            // Prefab� yok et
            Destroy(gameObject);
        }
    }
}
