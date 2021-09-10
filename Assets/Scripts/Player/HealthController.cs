using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public FilledBarController healthBarController;
    public float maxHealth;
    private float currentHealth = 100;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Min(value, maxHealth);
            healthBarController.CurrentValue = currentHealth;
        }
    }

    private void Awake()
    {
        currentHealth = 10f;
        healthBarController.MaxValue = maxHealth;
        healthBarController.CurrentValue = currentHealth;
    }
}
