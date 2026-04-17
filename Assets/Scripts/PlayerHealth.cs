using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 1. CRITICAL: Add this line at the top

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateUI();
        if (currentHealth <= 0) Die();
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("Player Died. Restarting Level...");

        // 2. Get the name of the current level and reload it
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        // Note: We don't necessarily need Destroy(gameObject) here 
        // because loading a scene clears out everything automatically.
    }
}