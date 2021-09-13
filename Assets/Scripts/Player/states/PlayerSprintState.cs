using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerBaseState
{
    private Vector3 moveDirection = Vector3.zero;
    
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.Animator.SetBool("isMoving", true);
        player.Animator.SetBool("isSprinting", true);
        player.StaminaController.CanRegenerateStamina = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.Animator.SetBool("isMoving", false);
        player.Animator.SetBool("isSprinting", false);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(!player.CharacterController.isGrounded) {
            player.SwitchState(player.fallingState);
        } else {
            MovePlayer(player);
            RotatePlayer(player);
            player.StaminaController.CurrentStamina -= player.sprintStamina * Time.deltaTime;
            if(player.StaminaController.CurrentStamina <= 0) {
                player.SwitchState(player.walkingState);
            }
        }
        
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
            player.SwitchState(player.idleState);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if(context.canceled) {
            player.SwitchState(player.walkingState);
        }
    }

    public override void OnLightAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (player.StaminaController.CurrentStamina > 0)
        {
            player.SwitchState(player.lightAttackState);
        }
    }

    public override void OnHeavyAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (player.StaminaController.CurrentStamina > 0)
        {
            player.SwitchState(player.heavyAttackState);
        }
    }
}