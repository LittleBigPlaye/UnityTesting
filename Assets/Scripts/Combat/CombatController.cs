using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour, IHitable
{
    [SerializeField] private WeaponHandle currentWeapon;
    [SerializeField] private LayerMask hitableMask;
    [SerializeField] private LayerMask obstacleMask;

    public Vector3 LastDamageOrigin {get; private set;}

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

    public void OnHit(Damage damage, Vector3 weaponPosition)
    {
        if (characterStateManager != null)
        {
            LastDamageOrigin = transform.position - weaponPosition;
            Debug.Log(LastDamageOrigin);
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
