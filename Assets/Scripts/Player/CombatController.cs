using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private bool canAttack;
    private Animator animator;
    private PlayerController playerController;

    public bool CanAttack {
        get{return canAttack;}
    }
    
    private void Awake() {
        canAttack = true;
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    public void TriggerAttack(bool isHeavyAttack) {
        animator.SetBool(nameof(isHeavyAttack), isHeavyAttack);
        animator.SetTrigger("attack");
        canAttack = false;
        playerController.IsAttacking = true;
    }

    public void ResetAttack() {
        canAttack = true;
    }

    public void EnableAttackTrigger() {
        //TODO: Activate the Weapon Logic to check if target has been hit
        //TODO: Disable Weapon Trail
    }

    public void DisableAttackTrigger() {
        //TODO: Disable the Weapon Logic to check if target has been hit
        //TODO: Disable Weapon Trail
    }

    // used to tell the player controller when it can start to move again
    public void EndAttack() {
        if(canAttack) {
            playerController.IsAttacking = false;
        }
    }
}
