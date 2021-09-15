using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private new Collider collider;
    public CombatController CombatController {get; set;}

    private void Awake() {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }

    public void SetTriggerState(bool isEnabled) {
        collider.enabled = isEnabled;
    }

    private void OnTriggerEnter(Collider other) {
        //TODO: Check if obstacle is between targetTo hit and sword
        Debug.Log(other.name);
        if(other.GetComponent<IHitable>() != null) {
            Damage damage = new Damage(100f);
            other.GetComponent<IHitable>().OnHit(damage);
        }
    }
}