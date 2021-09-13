using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour, IHitable
{
    public Weapon currentWeapon;
    public LayerMask hitableMask;
    public LayerMask obstacleMask;

    private CharacterStateManager characterStateManager;

    private void Awake() {
        currentWeapon.CombatController = this;
        characterStateManager = GetComponent<CharacterStateManager>();
    }

    public void EnableWeaponTrigger() {
        currentWeapon.SetTriggerState(true);
    }

    public void DisableWeaponTrigger() {
        currentWeapon.SetTriggerState(false);
    }

    public void OnHit(Damage damage)
    {
        if(characterStateManager != null) {
            characterStateManager.GetHit(damage.BaseDamage);
        }
    }
}
