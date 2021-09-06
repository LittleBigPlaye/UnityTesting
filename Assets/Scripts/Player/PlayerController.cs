using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(StaminaController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public Transform playerModel;
    public float walkingSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float playerRotationSpeed = 5f;
    public float gravity = 12f;


    public float sprintDelay = 1f;
    public float rollSpeed = 3f;
    public float dodgeSpeed = 2f;

    [Header("Camera")]
    public Transform camFollower;
    public float cameraRotationSpeed = 3f;
    public float cameraRotationLerp = .5f;

    [Header("LockOnSettings")]
    public float lockOnRadius = 5f;
    public float maxLockOnDistance = 15f;
    public float lockOnSpeed = 5f;
    public LayerMask layerMask;
    public LockOnCursorController lockOnCursor;


    private InputAsset inputAsset;
    // #region Controllers
    private CharacterController characterController;
    private HealthController healthController;
    private StaminaController staminaController;
    // #endregion
    private Animator animator;

    private bool isMoving;
    private Vector2 movementInput;
    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed = 0;

    private bool isSprinting;
    private Coroutine sprintCoroutine;

    private Vector2 cameraRotationInput = Vector2.zero;

    private Transform targetLockOnTransform = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthController = GetComponent<HealthController>();
        staminaController = GetComponent<StaminaController>();
        InitializeInputsBindings();

        animator = GetComponentInChildren<Animator>();

        isMoving = false;
        isSprinting = false;
        
    }

    private void InitializeInputsBindings()
    {
        inputAsset = new InputAsset();
        inputAsset.Player.Move.performed += OnMove;
        inputAsset.Player.Move.canceled += OnMove;

        inputAsset.Player.Sprint.performed += OnSprint;
        inputAsset.Player.Sprint.canceled += OnSprint;

        inputAsset.Player.RotateCamera.performed += OnRotateCamera;
        inputAsset.Player.RotateCamera.canceled += OnRotateCamera;

        inputAsset.Player.LockOn.performed += OnLockOn;

        inputAsset.Player.ChangeLockOn.performed += OnLockOnChange;
    }

    // #region Movement
    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
            //make sure to prevent magnitudes larger than 1
            movementInput = movementInput.magnitude > 1 ? movementInput.normalized : movementInput;
            isMoving = true;
            animator.SetBool("isMoving", true);

        }
        else if (context.canceled)
        {
            movementInput = Vector2.zero;
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
    }

    private void Move()
    {
        Vector2 currentInput = (isSprinting) ? movementInput.normalized : movementInput;
        Vector3 movement = camFollower.forward * currentInput.y + camFollower.right * currentInput.x;


        float targetMovementSpeed = (isSprinting) ? sprintingSpeed : walkingSpeed;
        currentSpeed = moveDirection.normalized.magnitude * targetMovementSpeed;

        moveDirection.x = movement.x * targetMovementSpeed;
        moveDirection.z = movement.z * targetMovementSpeed;

        if (IsInLockOnMode())
        {
            animator.SetFloat("walkSidewardSpeed", movementInput.x);
            animator.SetFloat("walkForwardSpeed", movementInput.y);
        }
        else
        {
            animator.SetFloat("walkSidewardSpeed", 0f);
            animator.SetFloat("walkForwardSpeed", movement.magnitude);
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (movementInput != Vector2.zero)
        {
            if (!IsInLockOnMode())
            {
                RotatePlayer(new Vector3(moveDirection.x, 0, moveDirection.z));
            } else {
                playerModel.LookAt(new Vector3(targetLockOnTransform.position.x, 0, targetLockOnTransform.position.z));
                //RotatePlayer(new Vector3(targetLockOnTransform.position.x, 0, targetLockOnTransform.position.y));
            }
            characterController.Move(moveDirection * Time.deltaTime);
        }

    }

    private void RotatePlayer(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target);
        if (Quaternion.Angle(playerModel.rotation, targetRotation) > .1f)
        {
            Quaternion nextRotation = Quaternion.Lerp(playerModel.rotation, targetRotation, Time.deltaTime * playerRotationSpeed);
            playerModel.rotation = nextRotation;
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (isMoving)
        {
            if (context.performed)
            {
                sprintCoroutine = StartCoroutine(StartSprintCountdown());
            }
            else
            {
                StopCoroutine(sprintCoroutine);
                if (!isSprinting)
                {
                    Debug.Log("Roll");
                }
                isSprinting = false;
                animator.SetBool("isSprinting", false);
            }
        }
        else
        {
            StopCoroutine(sprintCoroutine);
            isSprinting = false;
            animator.SetBool("isSprinting", false);
        }

    }

    IEnumerator StartSprintCountdown()
    {
        yield return new WaitForSeconds(sprintDelay);
        isSprinting = true;
        animator.SetBool("isSprinting", true);
    }
    // #endregion

    // #region Camera Handling
    private void OnRotateCamera(InputAction.CallbackContext context)
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

    private void RotateCamera()
    {
        if (cameraRotationInput != Vector2.zero)
        {
            // rotate the camera horizontally
            camFollower.rotation *= Quaternion.AngleAxis(cameraRotationInput.x * cameraRotationSpeed, Vector3.up);

            // rotate the camera vertically
            camFollower.rotation *= Quaternion.AngleAxis(cameraRotationInput.y * -cameraRotationSpeed, Vector3.right);

            RestrictCameraRotationAngles();
        }
    }

    private void RestrictCameraRotationAngles()
    {
        Vector3 angles = camFollower.localEulerAngles;
        angles.z = 0;
        float angle = camFollower.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        camFollower.localEulerAngles = angles;
    }

    void OnLockOn(InputAction.CallbackContext context)
    {
        if (!IsInLockOnMode())
        {
            List<Transform> lockOnTargets = GetNearbyLockOnTargets();
            Transform closestTarget = GetClosestLockOnTarget(lockOnTargets);
            if (closestTarget != null)
            {
                targetLockOnTransform = closestTarget;
                lockOnCursor.FollowTarget = closestTarget;
                targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = true;
            }
            else
            {
                camFollower.rotation = playerModel.rotation;
            }

        }
        else
        {
            targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = false;
            targetLockOnTransform = null;
            lockOnCursor.FollowTarget = null;
        }
    }

    void OnLockOnChange(InputAction.CallbackContext context)
    {
        if (IsInLockOnMode())
        {
            float input = context.ReadValue<float>();
            List<Transform> lockOnTargets = GetNearbyLockOnTargets();
            if (lockOnTargets.Count > 1)
            {
                List<Transform> orderedTargets = lockOnTargets.OrderBy(c => CalculateAngleBetweenTargetAndPlayer(c, true)).ToList();

                int currentTransformIndex = orderedTargets.IndexOf(targetLockOnTransform);
                int newTargetIndex = Mathf.Clamp(currentTransformIndex + (int)input, 0, orderedTargets.Count - 1);

                targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = false;
                targetLockOnTransform = orderedTargets[newTargetIndex];
                lockOnCursor.FollowTarget = orderedTargets[newTargetIndex];
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
        Vector2 currentPosition = new Vector2(camFollower.forward.x, camFollower.forward.z);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.z);
        Vector2 targetVector = new Vector2(targetPosition.x - currentPosition.x, targetPosition.y - currentPosition.y);
        float targetAngle = isSigned ? Vector2.SignedAngle(currentPosition, targetVector) : Vector2.Angle(currentPosition, targetVector);
        return targetAngle;
    }

    void UpdateLockOnRotation()
    {
        Vector3 relativePosition = targetLockOnTransform.position - camFollower.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition);
        camFollower.rotation = Quaternion.Lerp(camFollower.rotation, targetRotation, lockOnSpeed * Time.deltaTime);
        RestrictCameraRotationAngles();

        float distanceToLockOn = Vector3.Distance(camFollower.position, targetLockOnTransform.position);
        if (distanceToLockOn > maxLockOnDistance)
        {
            targetLockOnTransform.GetComponent<LockOnTargetController>().IsSelected = false;
            targetLockOnTransform = null;
            lockOnCursor.FollowTarget = null;
        }
    }

    private bool IsInLockOnMode()
    {
        return targetLockOnTransform != null;
    }

    // #endregion

    void Update()
    {
        if (IsInLockOnMode())
        {
            UpdateLockOnRotation();
        }
        else
        {
            RotateCamera();
        }
        Move();
    }

    private void OnEnable()
    {
        inputAsset.Enable();
    }
    private void OnDisable()
    {
        inputAsset.Disable();
    }
}
