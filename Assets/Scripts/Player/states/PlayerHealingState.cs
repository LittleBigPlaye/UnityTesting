using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.Animator.SetTrigger("heal");
    }

    public override void ExitState(PlayerStateManager player)
    {
        if (player.MovementInput == Vector2.zero)
        {
            player.SwitchState(player.idleState);
        }
        else
        {
            player.SwitchState(player.walkingState);
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnMove(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.started || context.performed)
        {
            player.Animator.SetBool("isMoving", true);
        }
        else
        {
            player.Animator.SetBool("isMoving", false);
        }
    }
}
