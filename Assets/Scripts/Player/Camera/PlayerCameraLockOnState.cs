using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerCameraLockOnState : PlayerCameraBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.lockOnCursor.FollowTarget = player.LockOnTarget;
        player.LockOnTarget.GetComponent<LockOnTargetController>().IsSelected = true;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.lockOnCursor.FollowTarget = null;
        player.SwitchState(player.freeRoamCameraState);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.LockOnTarget == null)
        {
            ExitState(player);
        }
        else
        {
            Vector3 relativePosition = player.LockOnTarget.position - player.camFollower.position;
            Quaternion targetRotation = Quaternion.LookRotation(relativePosition);
            player.camFollower.rotation = Quaternion.Lerp(player.camFollower.rotation, targetRotation, player.lockOnSpeed * Time.deltaTime);
            RestrictCameraRotationAngles(player);

            float distanceToLockOn = Vector3.Distance(player.camFollower.position, player.LockOnTarget.position);
            if (distanceToLockOn > player.maxLockOnDistance)
            {
                player.LockOnTarget.GetComponent<LockOnTargetController>().IsSelected = false;
                player.LockOnTarget = null;
            }
        }
    }

    public override void OnLockOn(InputAction.CallbackContext context, PlayerStateManager player)
    {
        player.LockOnTarget.GetComponent<LockOnTargetController>().IsSelected = false;
        player.LockOnTarget = null;
        ExitState(player);
        player.SwitchState(player.freeRoamCameraState);
    }

    public override void OnLockOnChange(InputAction.CallbackContext context, PlayerStateManager player)
    {
        float input = -context.ReadValue<float>();
        List<Transform> lockOnTargets = GetNearbyLockOnTargets(player);
        if (lockOnTargets.Count > 1)
        {
            List<Transform> orderedTargets = lockOnTargets.OrderBy(c => CalculateAngleBetweenTargetAndPlayer(c, player, true)).ToList();

            int currentTransformIndex = orderedTargets.IndexOf(player.LockOnTarget);
            int newTargetIndex = Mathf.Clamp(currentTransformIndex + (int)input, 0, orderedTargets.Count - 1);

            player.LockOnTarget.GetComponent<LockOnTargetController>().IsSelected = false;
            player.LockOnTarget = orderedTargets[newTargetIndex];
            player.lockOnCursor.FollowTarget = orderedTargets[newTargetIndex];
            player.LockOnTarget.GetComponent<LockOnTargetController>().IsSelected = true;
        }
    }
}