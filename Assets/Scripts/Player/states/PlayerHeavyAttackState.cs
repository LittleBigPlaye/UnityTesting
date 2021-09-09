using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeavyAttackState : PlayerBaseState
{
    bool isNextStateLightAttack = false;
    bool isNextStateHeavyAttack = false;

    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        if (previousState != player.lightAttackState && previousState != player.heavyAttackState)
        {
            player.Animator.SetTrigger("attack");
        }
        player.Animator.SetBool("isHeavyAttack", true);
    }

    public override void ExitState(PlayerStateManager player)
    {
        if (isNextStateLightAttack)
        {
            player.SwitchState(player.lightAttackState);
        }
        else if (isNextStateHeavyAttack)
        {
            player.SwitchState(player.heavyAttackState);
        }
        else
        {
            player.SwitchState(player.idleState);
        }
        isNextStateLightAttack = false;
        isNextStateHeavyAttack = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnLightAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        //TODO: Implement check if enough stamina is left
        if (!isNextStateHeavyAttack && !isNextStateLightAttack)
        {
            isNextStateLightAttack = true;
            player.Animator.SetTrigger("attack");
            player.Animator.SetBool("isHeavyAttack", false);
        }
    }

    public override void OnHeavyAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        //TODO: Implement check if enough stamina is left
        if (!isNextStateHeavyAttack && !isNextStateLightAttack)
        {
            isNextStateHeavyAttack = true;
            player.Animator.SetTrigger("attack");
            player.Animator.SetBool("isHeavyAttack", true);
        }
    }
}
