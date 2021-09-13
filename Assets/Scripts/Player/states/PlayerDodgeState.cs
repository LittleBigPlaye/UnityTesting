using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player, PlayerBaseState previousState)
    {
        player.Animator.SetTrigger("dodge");
        player.StaminaController.CanRegenerateStamina = false;
        player.StaminaController.CurrentStamina -= player.dodgeStamina;
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void EndStateByAnimation(PlayerStateManager player)
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
