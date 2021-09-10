using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Collider collider;
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
        if(other.GetComponent<IHitable>() != null) {
            other.GetComponent<IHitable>().OnHit();
        }
    }
}
