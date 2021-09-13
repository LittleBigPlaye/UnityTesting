using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightAttackState : PlayerBaseState
{
    bool isNextStateLightAttack = false;
    bool isNextStateHeavyAttack = false;


    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        if (previousState != player.lightAttackState && previousState != player.heavyAttackState)
        {
            player.Animator.SetTrigger("attack");
        }
        player.Animator.SetBool("isHeavyAttack", false);
        player.StaminaController.CanRegenerateStamina = false;
        player.StaminaController.CurrentStamina -= player.lightAttackStamina;
    }

    public override void ExitState(PlayerStateManager player)
    {

        isNextStateLightAttack = false;
        isNextStateHeavyAttack = false;
    }

    public override void EndStateByAnimation(PlayerStateManager player)
    {
        if (isNextStateLightAttack)
        {
            player.SwitchState(player.lightAttackState);
        }
        else if (isNextStateHeavyAttack)
        {
            player.SwitchState(player.heavyAttackState);
        }
        else
        {
            player.SwitchState(player.idleState);
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {

    }

    public override void OnLightAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (!isNextStateHeavyAttack && !isNextStateLightAttack && player.StaminaController.CurrentStamina > 0)
        {
            isNextStateLightAttack = true;
            player.Animator.SetTrigger("attack");
            player.Animator.SetBool("isHeavyAttack", false);
        }
    }

    public override void OnHeavyAttack(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (!isNextStateHeavyAttack && !isNextStateLightAttack && player.StaminaController.CurrentStamina > 0)
        {
            isNextStateHeavyAttack = true;
            player.Animator.SetTrigger("attack");
            player.Animator.SetBool("isHeavyAttack", true);
        }
    }
}
