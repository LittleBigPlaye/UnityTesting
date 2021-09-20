using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private Vector3 damageOrigin;
    
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        damageOrigin = player.CombatController.LastDamageOrigin.normalized * 20f;
        player.StaminaController.CanRegenerateStamina = false;
        player.Animator.SetTrigger("getHit");
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void EndStateByAnimation(PlayerStateManager player)
    {
        if (player.HealthController.CurrentHealth > 0)
        {
            player.SwitchState(player.idleState);
        }
        else
        {
            player.Animator.SetBool("isDead", true);
            player.SwitchState(player.deathState);
        }
    }


    public override void UpdateState(PlayerStateManager player)
    {
        player.Move(-damageOrigin);
    }

    public override void GetHit(PlayerStateManager player)
    {
        
    }
}
