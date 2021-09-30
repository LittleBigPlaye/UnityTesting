
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private WeaponHandle currentWeapon;
    [SerializeField] private ShieldHandle currentShield;
    [SerializeField] private LayerMask hitableMask;
    [SerializeField] private LayerMask obstacleMask;

    public LayerMask ObstacleMask { get => obstacleMask; }
    public LayerMask HitableMask { get => hitableMask; }
    public Collider Collider {get; private set;}


    [Tooltip("Hit Box for default hit detection")]
    [SerializeField] private Collider hitBox;
    [Tooltip("Hit Box for reduced Hit Detection, when player is Blocking. Should be located behind the blocking Hit Box.")]
    [SerializeField] private Collider reducedHitBox;


    public Vector3 LastDamageOrigin { get; private set; }

    private CharacterStateManager characterStateManager;
    private CharacterStatsScriptableObject characterStats;

    private void Awake()
    {
        if (currentWeapon != null) currentWeapon.CombatController = this;
        if (currentShield != null)
        {
            currentShield.CombatController = this;
            currentShield.enabled = false;
        }

        characterStateManager = GetComponent<CharacterStateManager>();
        characterStats = characterStateManager?.CharacterStats;
        hitBox.enabled = true;
        reducedHitBox.enabled = false;
        
        Collider = hitBox;
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
            characterStateManager.GetHit(CalculateResultingDamage(damage));
        }
    }

    public void Block(bool isBlocking)
    {
        hitBox.enabled = !isBlocking;
        reducedHitBox.enabled = isBlocking;
        currentShield.enabled = true;
        
        Collider = isBlocking ? reducedHitBox : hitBox;
    }

    private float CalculateResultingDamage(Damage damage)
    {
        float resultingDamage = 10f;
        resultingDamage =
            damage.BaseDamage * characterStats.baseDamageVulnerability;
        return resultingDamage;
    }
}
