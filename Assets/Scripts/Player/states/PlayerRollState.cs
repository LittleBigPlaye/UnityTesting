using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.Animator.SetTrigger("roll");
        player.Animator.SetBool("isMoving", true);
        player.StaminaController.CanRegenerateStamina = false;
        player.StaminaController.CurrentStamina -= player.rollStamina;


        //rotate player immediately towards the roll direction
        Transform camFollower = player.camFollower;
        Vector2 movementInput = player.MovementInput;

        Vector3 faceDirection = camFollower.forward * movementInput.y + camFollower.right * movementInput.x;
        Quaternion targetRotation = Quaternion.LookRotation(faceDirection);
        player.transform.rotation = targetRotation;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.Animator.SetBool("isMoving", false);
    }

    public override void EndStateByAnimation(PlayerStateManager player)
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
        Vector3 rollDirection = player.transform.forward * player.rollSpeed;
        rollDirection.y = 0;
        player.Move(rollDirection);
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
