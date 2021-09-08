using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerBaseState
{
    private Vector3 moveDirection = Vector3.zero;
    
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.SetBool("isSprinting", true);
    }

    public override void ExitState(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(!player.CharacterController.isGrounded) {
            player.Animator.SetBool("isSprinting", false);
            player.SwitchState(player.fallingState);
        }
        MovePlayer(player);
        RotatePlayer(player);
    }

    private void MovePlayer(PlayerStateManager player) {
        Transform camFollower = player.camFollower;
        Vector2 movementInput = player.MovementInput.normalized;

        moveDirection = camFollower.forward * movementInput.y + camFollower.right * movementInput.x;
        
        moveDirection.x *= player.sprintingSpeed;
        moveDirection.z *= player.sprintingSpeed;
        player.Move(moveDirection);
    }

    private void RotatePlayer(PlayerStateManager player) {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        if(Quaternion.Angle(player.transform.rotation, targetRotation) > .1f) {
            Quaternion nextRotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * player.playerRotationSpeed);
            player.transform.rotation = nextRotation;
        }
    }

    public override void OnMove(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if(context.canceled) {
            player.Animator.SetBool("isSprinting",false);
            player.SwitchState(player.idleState);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if(context.canceled) {
            player.Animator.SetBool("isSprinting", false);
            player.SwitchState(player.walkingState);
        }
    }
}