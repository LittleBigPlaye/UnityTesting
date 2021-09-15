using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] FilledBarController healthBarController;
    [SerializeField] float maxHealth;

    private float currentHealth = 100;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            healthBarController.CurrentValue = currentHealth;
        }
    }

    private void Awake()
    {
        healthBarController.MaxValue = maxHealth;
        currentHealth = maxHealth;
        healthBarController.CurrentValue = currentHealth;
    }
}
