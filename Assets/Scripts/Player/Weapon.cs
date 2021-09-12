using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Collider weaponCollider;
    public CombatController CombatController {get; set;}

    private void Awake() {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    public void SetTriggerState(bool isEnabled) {
        weaponCollider.enabled = isEnabled;
    }

    private void OnTriggerEnter(Collider other) {
        //TODO: Check if obstacle is between targetTo hit and sword
        if(other.GetComponent<IHitable>() != null) {
            other.GetComponent<IHitable>().OnHit();
        }
    }
}