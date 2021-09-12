using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.StaminaController.CanRegenerateStamina = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        if (player.HealthController.CurrentHealth > 0)
        {
            player.SwitchState(player.idleState);
        }
        else
        {
            player.SwitchState(player.deathState);
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }
}
