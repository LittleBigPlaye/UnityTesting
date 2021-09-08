using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHardLandingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.SetBool("isGrounded", true);
        player.Animator.SetBool("isGrounded", true);
        Gamepad.current.SetMotorSpeeds(0.345f, 0.234f);
    }

    public override void ExitState(PlayerStateManager player)
    {
        Gamepad.current.SetMotorSpeeds(0f, 0f);
        player.Animator.SetBool("isLongFall", false);
        if (player.MovementInput == Vector2.zero)
        {

            player.SwitchState(player.idleState);
        }
        else if (player.MovementInput != Vector2.zero)
        {

            player.SwitchState(player.walkingState);
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Move(Vector3.zero);
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
