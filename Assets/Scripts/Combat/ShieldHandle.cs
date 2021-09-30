using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHandle : MonoBehaviour, IHitable
{
    private Collider shieldTrigger;
    public CombatController CombatController{get; set;}

    public void OnHit(Damage damage, Vector3 weaponPosition)
    {
        //TODO: Calculate Damage that has been Blocked by the shield
        //Pass remaining damage to the combatController
    }

    private void Awake() {
        shieldTrigger = GetComponent<Collider>();
        shieldTrigger.enabled = false;

        CombatController = GetComponentInParent<CombatController>();
    }

    private void OnEnable() {
        shieldTrigger.enabled = true;
    }

    private void OnDisable() {
        shieldTrigger.enabled = false;    
    }

}
