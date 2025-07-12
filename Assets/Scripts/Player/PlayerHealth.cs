using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Elements")]
    public Image healthBar; 

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);

    }


}
