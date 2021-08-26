using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public FilledBarController barController;

    public float maxHealth = 100f;
    private float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        barController.maxValue = maxHealth;
        barController.currentValue = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit() {
        currentHealth -= 10;
        currentHealth = Mathf.Max(0, currentHealth);
        barController.currentValue = currentHealth;
        Debug.Log("Test");
    }

    public void Heal() {
        currentHealth += 10;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        barController.currentValue = currentHealth;
    }
}
