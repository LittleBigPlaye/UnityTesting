using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.StaminaController.CanRegenerateStamina = true;
    }


    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.CharacterController.isGrounded)
        {
            player.SwitchState(player.fallingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void OnMove(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.performed || context.started)
        {
            player.SwitchState(player.walkingState);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.performed && player.StaminaController.CurrentStamina > 0)
        {
            player.SwitchState(player.dodgeState);
        }
    }

    public override void OnUseItem(InputAction.CallbackContext context, PlayerStateManager player)
    {
        //TODO: Check if player has enough flasks for healing
        if (true)
        {
            player.SwitchState(player.healingState);
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
