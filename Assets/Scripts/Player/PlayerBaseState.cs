using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player);

    public virtual void OnMove(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnSprint(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnJump(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnLightAttack(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnHeavyAttack(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnBlock(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnParry(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnUseItem(InputAction.CallbackContext context, PlayerStateManager player) {}


}
