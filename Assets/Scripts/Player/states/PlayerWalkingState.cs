using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerBaseState
{
    private Vector3 moveDirection = Vector3.zero;
    private bool isSprintKeyPressed = false;
    private float currentSprintKeyTime = 0f;

    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.StaminaController.CanRegenerateStamina = true;
        player.Animator.SetBool("isMoving", true);
        player.Animator.SetFloat("walkSidewardSpeed", 0f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.CharacterController.isGrounded)
        {
            ExitState(player);
            player.SwitchState(player.fallingState);
        }

        MovePlayer(player);
        RotatePlayer(player);
        Debug.Log(Vector3.Angle(moveDirection, player.transform.forward));

        if (isSprintKeyPressed && currentSprintKeyTime < player.sprintDelay)
        {
            currentSprintKeyTime += Time.deltaTime;
        }
        else if (currentSprintKeyTime >= player.sprintDelay)
        {
            isSprintKeyPressed = false;
            currentSprintKeyTime = 0f;
            player.SwitchState(player.sprintState);
        }
    }

    private void MovePlayer(PlayerStateManager player)
    {
        Transform camFollower = player.camFollower;
        Vector2 movementInput = player.MovementInput;

        moveDirection = camFollower.forward * movementInput.y + camFollower.right * movementInput.x;

        moveDirection.x *= player.walkingSpeed;
        moveDirection.z *= player.walkingSpeed;
        player.Move(moveDirection);

        if (player.LockOnTarget != null)
        {
            player.Animator.SetFloat("walkSidewardSpeed", movementInput.x);
            player.Animator.SetFloat("walkForwardSpeed", movementInput.y);
        }
        else
        {
            player.Animator.SetFloat("walkSidewardSpeed", 0f);
            float movementSpeedPercentage = Mathf.Clamp(player.CharacterController.velocity.magnitude / player.walkingSpeed, 0, 1);
            player.Animator.SetFloat("walkForwardSpeed", movementSpeedPercentage);
        }
    }

    private void RotatePlayer(PlayerStateManager player)
    {
        if (player.LockOnTarget != null)
        {
            player.transform.LookAt(new Vector3(player.LockOnTarget.position.x, 0f, player.LockOnTarget.position.z));
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            if (Quaternion.Angle(player.transform.rotation, targetRotation) > .1f)
            {
                Quaternion nextRotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * player.playerRotationSpeed);
                player.transform.rotation = nextRotation;
            }
        }


    }



    public override void ExitState(PlayerStateManager player)
    {
        isSprintKeyPressed = false;
        currentSprintKeyTime = 0f;
        player.Animator.SetBool("isMoving", false);
    }

    public override void OnMove(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.canceled)
        {
            player.Animator.SetBool("isMoving", false);
            player.SwitchState(player.idleState);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.performed && player.StaminaController.CurrentStamina > 0)
        {
            isSprintKeyPressed = true;
        }
        else
        {
            if (isSprintKeyPressed && currentSprintKeyTime < player.sprintDelay && player.StaminaController.CurrentStamina > 0)
            {
                isSprintKeyPressed = false;
                currentSprintKeyTime = 0f;
                player.SwitchState(player.rollState);
            }
        }
    }

    public override void OnUseItem(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (player.InventoryController.CurrentNumberOfPotions > 0)
        {
            ExitState(player);
            player.SwitchState(player.healingState);
        }
    }

    public override void OnLightAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (player.StaminaController.CurrentStamina > 0)
        {
            player.SwitchState(player.lightAttackState);
            ExitState(player);
        }
    }

    public override void OnHeavyAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (player.StaminaController.CurrentStamina > 0)
        {
            player.SwitchState(player.heavyAttackState);
            ExitState(player);
        }
    }
}
