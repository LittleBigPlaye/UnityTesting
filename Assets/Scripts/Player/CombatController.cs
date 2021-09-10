using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Weapon currentWeapon;
    public LayerMask hitableMask;
    public LayerMask obstacleMask;

    private void Awake() {
        currentWeapon.CombatController = this;
    }

    public void EnableWeaponTrigger() {
        currentWeapon.SetTriggerState(true);
    }

    public void DisableWeaponTrigger() {
        currentWeapon.SetTriggerState(false);
    }
}
