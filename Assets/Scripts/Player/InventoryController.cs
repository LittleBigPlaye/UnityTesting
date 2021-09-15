using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] int maxNumberOfPotions = 5;
    [SerializeField] float potionHealthRestoreValue = 50f;

    public float PotionHealthRestoreValue {get => potionHealthRestoreValue;}

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
