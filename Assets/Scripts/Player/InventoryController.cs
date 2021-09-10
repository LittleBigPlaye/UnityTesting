using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public int maxNumberOfPotions = 5;
    public float potionHealthRestoreValue = 50f;

    private int currentNumberOfPotions;
    public int CurrentNumberOfPotions
    {
        get => currentNumberOfPotions;
        set
        {
            currentNumberOfPotions = Mathf.Clamp(value, 0, maxNumberOfPotions);
        }
    }

    private void Awake() {
        currentNumberOfPotions = maxNumberOfPotions;
    }
}
