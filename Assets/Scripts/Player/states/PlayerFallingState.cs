using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallingState : PlayerBaseState
{
    private float currentFallingTime = 0f;

    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        currentFallingTime = 0f;
        player.Animator.SetBool("isGrounded", false);
    }

    public override void ExitState(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        currentFallingTime += Time.deltaTime;
        if(currentFallingTime >= player.hardLandingDelay) {
            player.Animator.SetBool("isLongFall", true);
        }
        if (player.CharacterController.isGrounded && currentFallingTime >= player.hardLandingDelay)
        {
            player.SwitchState(player.hardLandingState);
        }
        else if (player.CharacterController.isGrounded && player.MovementInput == Vector2.zero)
        {
            player.Animator.SetBool("isGrounded", true);
            player.SwitchState(player.idleState);
        }
        else if (player.CharacterController.isGrounded && player.MovementInput != Vector2.zero)
        {
            player.Animator.SetBool("isGrounded", true);
            player.SwitchState(player.walkingState);
        }
        player.Move(new Vector3(0, 0, 0));
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
            player.Animator.SetBool("isSprinting", false);
        }
    }
}
