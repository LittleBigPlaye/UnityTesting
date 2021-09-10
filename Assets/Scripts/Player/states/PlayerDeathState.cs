using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.StaminaController.CanRegenerateStamina = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }
}
