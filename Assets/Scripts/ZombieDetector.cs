using System;
using UnityEngine;
using UnityEngine.UI;

public class ZombieDetector : MonoBehaviour
{
    [SerializeField] Image healthBar;
    private float maxHealth = 1;
    private float currentHealth;

    public event Action DeathEvent;

    public void Restart()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth;
    }

    private void Death()
    {
        DeathEvent?.Invoke();
        Debug.Log("Game Over");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombie"))
        {
            currentHealth -= 0.15f;
            healthBar.fillAmount = currentHealth;
            if(currentHealth <= 0)
            {
                Death();
            }
        }
    }
}
