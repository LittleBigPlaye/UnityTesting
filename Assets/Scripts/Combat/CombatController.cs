using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour, IHitable
{
    public WeaponHandle currentWeapon;
    public LayerMask hitableMask;
    public LayerMask obstacleMask;

    private CharacterStateManager characterStateManager;
    private CharacterStatsScriptableObject characterStats;

    private void Awake()
    {
        currentWeapon.CombatController = this;
        characterStateManager = GetComponent<CharacterStateManager>();
        characterStats = characterStateManager?.CharacterStats;
    }

    public void EnableWeaponTrigger()
    {
        currentWeapon.SetTriggerState(true);
    }

    public void DisableWeaponTrigger()
    {
        currentWeapon.SetTriggerState(false);
    }

    public void OnHit(Damage damage)
    {
        if (characterStateManager != null)
        {
            characterStateManager.GetHit(calculateResultingDamage(damage));
        }
    }

    private float calculateResultingDamage(Damage damage)
    {
        float resultingDamage = 10f;
        resultingDamage =
            damage.BaseDamage * characterStats.baseDamageVulnerability;
        return resultingDamage;
    }
}
