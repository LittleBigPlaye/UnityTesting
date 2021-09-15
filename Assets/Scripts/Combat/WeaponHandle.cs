using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandle : MonoBehaviour
{
    [SerializeField] WeaponScriptableObject weaponStats;
    private new Collider collider;
    public CombatController CombatController {get; set;}

    private TrailRenderer trailRenderer;

    private void Awake() {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    public void SetTriggerState(bool isEnabled) {
        collider.enabled = isEnabled;
        trailRenderer.enabled = isEnabled;
    }

    private void OnTriggerEnter(Collider other) {
        //TODO: Check if obstacle is between targetTo hit and sword
        Debug.Log(other.name);
        if(other.GetComponent<IHitable>() != null) {
            Damage damage = new Damage(weaponStats.baseDamage);
            other.GetComponent<IHitable>().OnHit(damage);
        }
    }
}