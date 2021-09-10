using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaController : MonoBehaviour
{

    public float regenerationDelay = 4f;
    public float regenerationSpeed = 5f;

    public float maxStamina = 100f;

    private bool canRegenerateStamina;
    public bool CanRegenerateStamina
    {
        get => canRegenerateStamina; set
        {
            if (!value)
            {
                currentStaminaDelay = 0f;
            }
            canRegenerateStamina = value;
        }
    }

    private float currentStamina;
    public float CurrentStamina
    {
        get => currentStamina;
        set
        {
            if (value < currentStamina)
            {
                currentStaminaDelay = 0f;
            }
            currentStamina = value;
            if (filledBarController != null)
            {
                filledBarController.CurrentValue = value;
            }
        }
    }
    private float currentStaminaDelay = 0f;
    private FilledBarController filledBarController;

    private void Awake()
    {
        filledBarController = FindObjectOfType<FilledBarController>();
        filledBarController.MaxValue = maxStamina;
        CurrentStamina = maxStamina;
        CanRegenerateStamina = true;
    }

    private void Update()
    {
        if (CanRegenerateStamina && CurrentStamina < maxStamina)
        {
            if (currentStaminaDelay >= regenerationDelay)
            {
                CurrentStamina = Mathf.Min(CurrentStamina + regenerationSpeed * Time.deltaTime, maxStamina);
            }
            else
            {
                currentStaminaDelay += Time.deltaTime;
            }
        }
    }
}
