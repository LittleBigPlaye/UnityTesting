using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(PlayerStateManager player)
    {
        //TODO: Check if the player has more than zero lifepoints
        if (true)
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
