using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.StaminaController.CanRegenerateStamina = true;
        player.Animator.SetFloat("IKLeftFootWeight", 1f);
        player.Animator.SetFloat("IKRightFootWeight", 1f);
    }


    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.CharacterController.isGrounded)
        {
            player.SwitchState(player.fallingState);
            ExitState(player);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.Animator.SetFloat("IKLeftFootWeight", 0f);
        player.Animator.SetFloat("IKRightFootWeight", 0f);
    }

    public override void OnMove(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.performed || context.started)
        {
            player.SwitchState(player.walkingState);
            ExitState(player);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.performed && player.StaminaController.CurrentStamina > 0)
        {
            player.SwitchState(player.dodgeState);
            ExitState(player);
        }
    }

    public override void OnUseItem(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (player.InventoryController.CurrentNumberOfPotions > 0)
        {
            player.SwitchState(player.healingState);
            ExitState(player);
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
