using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IHitable
{
    private CombatController combatController;
    
    private void Awake() {
        combatController = GetComponentInParent<CombatController>();
    }
    
    public void OnHit(Damage damage, Vector3 weaponPosition)
    {
        combatController.OnHit(damage, weaponPosition);
    }
}
