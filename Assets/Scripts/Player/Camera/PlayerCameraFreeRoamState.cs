using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraFreeRoamState : PlayerCameraBaseState
{
    private Vector2 cameraRotationInput;

    public override void EnterState(PlayerStateManager player)
    {
        player.LockOnTarget = null;
    }

    public override void ExitState(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (cameraRotationInput != Vector2.zero)
        {
            // rotate the camera horizontally
            player.camFollower.rotation *= Quaternion.AngleAxis(cameraRotationInput.x * player.cameraRotationSpeed * Time.deltaTime, Vector3.up);

            // rotate the camera vertically
            player.camFollower.rotation *= Quaternion.AngleAxis(cameraRotationInput.y * -player.cameraRotationSpeed * Time.deltaTime, Vector3.right);

            RestrictCameraRotationAngles(player);
        }
    }

    public override void OnRotateCamera(InputAction.CallbackContext context, PlayerStateManager player)
    {
        if (context.performed)
        {
            cameraRotationInput = context.ReadValue<Vector2>();
        }
        else
        {
            cameraRotationInput = Vector2.zero;
        }
    }

    public override void OnLockOn(InputAction.CallbackContext context, PlayerStateManager player)
    {
        List<Transform> lockOnTargets = GetNearbyLockOnTargets(player);
            Transform closestTarget = GetClosestLockOnTarget(lockOnTargets, player);
            if (closestTarget != null)
            {
                player.LockOnTarget = closestTarget;
                player.SwitchState(player.lockOnCameraState);   
            }
            else
            {
                player.camFollower.rotation = player.transform.rotation;
            }
    }

    private Transform GetClosestLockOnTarget(List<Transform> targetTransforms, PlayerStateManager player)
    {
        Transform closestTarget = null;
        float lowestAngle = 360;
        foreach (Transform t in targetTransforms)
        {
            float currentAngle = CalculateAngleBetweenTargetAndPlayer(t, player);
            if (currentAngle < lowestAngle)
            {
                closestTarget = t;
                lowestAngle = currentAngle;
            }
        }
        return closestTarget;
    }

    
}
