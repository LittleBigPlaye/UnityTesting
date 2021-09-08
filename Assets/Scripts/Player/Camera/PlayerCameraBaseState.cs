using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public abstract class PlayerCameraBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player);

    public virtual void OnRotateCamera(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnLockOn(InputAction.CallbackContext context, PlayerStateManager player) {}
    public virtual void OnLockOnChange(InputAction.CallbackContext context, PlayerStateManager player) {}

    protected List<Transform> GetNearbyLockOnTargets(PlayerStateManager player)
    {
        Collider[] targetsInRadius = Physics.OverlapSphere(player.transform.position, player.lockOnRadius, player.layerMask);
        //make sure to only include transforms that are actually a lock on target
        List<Transform> targetTransforms = targetsInRadius.Where(c => c.GetComponent<LockOnTargetController>() != null).Select(c => c.transform).ToList();
        return targetTransforms;
    }

    protected void RestrictCameraRotationAngles(PlayerStateManager player)
    {
        Vector3 angles = player.camFollower.localEulerAngles;
        angles.z = 0;
        float angle = player.camFollower.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        player.camFollower.localEulerAngles = angles;
    }

    protected float CalculateAngleBetweenTargetAndPlayer(Transform target, PlayerStateManager player, bool isSigned = false)
    {
        Vector2 currentPosition = new Vector2(player.camFollower.forward.x, player.camFollower.forward.z);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.z);
        Vector2 targetVector = new Vector2(targetPosition.x - currentPosition.x, targetPosition.y - currentPosition.y);
        float targetAngle = isSigned ? Vector2.SignedAngle(currentPosition, targetVector) : Vector2.Angle(currentPosition, targetVector);
        return targetAngle;
    }
}
