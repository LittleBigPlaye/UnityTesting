using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    private bool isMoving = false;
    
    public override void EnterState(PlayerStateManager player)
    {

    }


    public override void UpdateState(PlayerStateManager player)
    {
        if(!player.CharacterController.isGrounded) {
            player.SwitchState(player.fallingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void OnMove(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if(context.performed || context.started) {
            player.SwitchState(player.walkingState);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if(context.performed) {
            player.SwitchState(player.dodgeState);
        }
    }
}
