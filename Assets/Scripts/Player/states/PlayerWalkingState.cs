using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerBaseState
{
    private Vector3 moveDirection = Vector3.zero;
    private bool isSprintKeyPressed = false;
    private float currentSprintKeyTime = 0f;

    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.SetBool("isMoving", true);
        player.Animator.SetFloat("walkSidewardSpeed", 0f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(!player.CharacterController.isGrounded) {
            ExitState(player);
            player.SwitchState(player.fallingState);
        }
        
        MovePlayer(player);
        RotatePlayer(player);

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

        float movementSpeedPercentage = Mathf.Clamp(player.CharacterController.velocity.magnitude / player.walkingSpeed, 0, 1);
        player.Animator.SetFloat("walkForwardSpeed", movementSpeedPercentage);
    }

    private void RotatePlayer(PlayerStateManager player)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        if (Quaternion.Angle(player.transform.rotation, targetRotation) > .1f)
        {
            Quaternion nextRotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * player.playerRotationSpeed);
            player.transform.rotation = nextRotation;
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        isSprintKeyPressed = false;
        currentSprintKeyTime = 0f;
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
        if (context.performed)
        {
            isSprintKeyPressed = true;
        }
        else
        {
            if (isSprintKeyPressed && currentSprintKeyTime < player.sprintDelay)
            {
                isSprintKeyPressed = false;
                currentSprintKeyTime = 0f;
                player.SwitchState(player.rollState);
            }
        }
    }





}
