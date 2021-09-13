using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.StaminaController.CanRegenerateStamina = false;
        player.Animator.SetBool("isDead", true);
    }

    public override void ExitState(PlayerStateManager player)
    {
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void GetHit(PlayerStateManager player)
    {
    }
}
