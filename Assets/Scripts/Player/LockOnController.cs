using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class LockOnController : MonoBehaviour
{
    public float lockOnRadius = 5f;
    public float maxLockOnDistance = 15f;
    public float lockOnSpeed = 5f;
    public Transform cameraFollower;
    public LayerMask layerMask;
    public InputActionAsset inputActionAsset;
    private InputActionMap actionMap;
    private InputAction lockOnAction;
    private InputAction changeLockOn;

    private Transform targetLockOnTransform;


    void Awake()
    {
        actionMap = inputActionAsset.FindActionMap("Player");
        lockOnAction = actionMap.FindAction("LockOn");
        changeLockOn = actionMap.FindAction("ChangeLockOn");
        lockOnAction.performed += OnLockOn;
        changeLockOn.performed += OnLockOnChange;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targetLockOnTransform != null)
        {
            UpdateLockOnRotation();
        }
    }

    void UpdateLockOnRotation()
    {
        Vector3 relativePosition = targetLockOnTransform.position - cameraFollower.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition);
        cameraFollower.rotation = Quaternion.Lerp(cameraFollower.rotation, targetRotation, lockOnSpeed * Time.deltaTime);

        float distanceToLockOn = Vector3.Distance(cameraFollower.position, targetLockOnTransform.position);
        if (distanceToLockOn > maxLockOnDistance)
        {
            targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = false;
            targetLockOnTransform = null;
        }
    }

    void OnLockOn(InputAction.CallbackContext context)
    {
        if (targetLockOnTransform == null)
        {
            List<Transform> lockOnTargets = GetNearbyLockOnTargets();
            Transform closestTarget = GetClosestLockOnTarget(lockOnTargets);
            if (closestTarget != null)
            {
                targetLockOnTransform = closestTarget;
                targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = true;
            }

        }
        else
        {
            targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = false;
            targetLockOnTransform = null;
        }
    }

    void OnLockOnChange(InputAction.CallbackContext context)
    {

        if (targetLockOnTransform != null)
        {
            Vector2 input = context.ReadValue<Vector2>().normalized * -1;
            List<Transform> lockOnTargets = GetNearbyLockOnTargets();
            if (lockOnTargets.Count > 1)
            {
                List<Transform> orderedTargets = lockOnTargets.OrderBy(c => CalculateAngleBetweenTargetAndPlayer(c, true)).ToList();

                int currentTransformIndex = orderedTargets.IndexOf(targetLockOnTransform);
                int newTargetIndex = Mathf.Clamp(currentTransformIndex + (int)input.x, 0, orderedTargets.Count - 1);

                targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = false;
                targetLockOnTransform = orderedTargets[newTargetIndex];
                targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = true;
            }

        }
    }

    private List<Transform> GetNearbyLockOnTargets()
    {
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, lockOnRadius, layerMask);
        //make sure to only include transforms that are actually a lock on target
        List<Transform> targetTransforms = targetsInRadius.Where(c => c.GetComponent<LockOnTargetController>() != null).Select(c => c.transform).ToList();
        return targetTransforms;
    }

    private Transform GetClosestLockOnTarget(List<Transform> targetTransforms)
    {
        Transform closestTarget = null;
        float lowestAngle = 360;
        foreach (Transform t in targetTransforms)
        {
            float currentAngle = CalculateAngleBetweenTargetAndPlayer(t);
            if (currentAngle < lowestAngle)
            {
                closestTarget = t;
                lowestAngle = currentAngle;
            }
        }
        return closestTarget;
    }

    private float CalculateAngleBetweenTargetAndPlayer(Transform target, bool isSigned = false)
    {
        Vector2 currentPosition = new Vector2(cameraFollower.forward.x, cameraFollower.forward.z);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.z);
        Vector2 vectorToCalculate = new Vector2(targetPosition.x - currentPosition.x, targetPosition.y - currentPosition.y);
        float targetAngle = isSigned ? Vector2.SignedAngle(currentPosition, vectorToCalculate) : Vector2.Angle(currentPosition, vectorToCalculate);
        return targetAngle;
    }

    private void OnDisable()
    {
        lockOnAction.Disable();
        changeLockOn.Disable();
    }

    private void OnEnable()
    {
        lockOnAction.Enable();
        changeLockOn.Enable();
    }
}
