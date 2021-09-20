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
        //TODO: Tell Game Manager to Show "You Died" and Reload Scene afterwards
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.CharacterController.isGrounded)
        {
            player.Move(Vector3.zero);
        }
    }

    public override void GetHit(PlayerStateManager player)
    {
    }
}
