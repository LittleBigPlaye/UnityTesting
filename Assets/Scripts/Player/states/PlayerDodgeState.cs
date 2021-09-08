using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.SetTrigger("dodge");
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.SwitchState(player.idleState);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        Vector3 dodgeDirection = -player.transform.forward * player.dodgeSpeed;
        dodgeDirection.y = 0;
        player.Move(dodgeDirection); 
    }
}
