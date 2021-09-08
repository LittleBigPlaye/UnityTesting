using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
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
    public float hardLandingDelay = 1f;

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

    private InputAsset input;

    private Animator animator;
    public Animator Animator { get => animator; }

    private CharacterController characterController;
    public CharacterController CharacterController { get => characterController; }


    /* #region Player States */
    private PlayerBaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkingState walkingState = new PlayerWalkingState();

    public PlayerDodgeState dodgeState = new PlayerDodgeState();
    public PlayerSprintState sprintState = new PlayerSprintState();
    public PlayerRollState rollState = new PlayerRollState();

    public PlayerFallingState fallingState = new PlayerFallingState();
    public PlayerHardLandingState hardLandingState = new PlayerHardLandingState();
    /* #endregion */

    /* #region Camera States */
    private PlayerCameraBaseState currentCameraState;
    public PlayerCameraFreeRoamState freeRoamCameraState = new PlayerCameraFreeRoamState();
    public PlayerCameraLockOnState lockOnCameraState = new PlayerCameraLockOnState();
    /* #endregion */

    private Vector2 movementInput = Vector2.zero;
    public Vector2 MovementInput { get => movementInput; }

    private Transform lockOnTarget = null;
    public Transform LockOnTarget {get; set;}

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = idleState;
        currentState.EnterState(this);

        currentCameraState = freeRoamCameraState;
        currentCameraState.EnterState(this);

        characterController = GetComponentInParent<CharacterController>();
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        input = new InputAsset();

        input.Player.Move.started += OnMove;
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;

        input.Player.Sprint.performed += OnSprint;
        input.Player.Sprint.canceled += OnSprint;

        input.Player.RotateCamera.performed += OnRotateCamera;
        input.Player.RotateCamera.canceled += OnRotateCamera;
        input.Player.LockOn.performed += OnLockOn;
        input.Player.ChangeLockOn.performed += OnLockOnChange;
    }

    private void Update()
    {
        currentState.UpdateState(this);
        currentCameraState.UpdateState(this);
    }

    public void Move(Vector3 movementDirection)
    {
        movementDirection += Physics.gravity;
        characterController.Move(movementDirection * Time.deltaTime);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void SwitchState(PlayerCameraBaseState state) {
        currentCameraState = state;
        currentCameraState.EnterState(this);
    }

    public void EndState()
    {
        currentState.ExitState(this);
    }

    /* #region inputs */
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
            movementInput = movementInput.magnitude > 1 ? movementInput.normalized : movementInput;
        }
        else
        {
            movementInput = Vector2.zero;
        }
        currentState.OnMove(context, this);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        currentState.OnSprint(context, this);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        currentState.OnJump(context, this);
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        currentState.OnLightAttack(context, this);
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        currentState.OnHeavyAttack(context, this);
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        currentState.OnBlock(context, this);
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        currentState.OnParry(context, this);
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        currentState.OnUseItem(context, this);
    }

    public void OnRotateCamera(InputAction.CallbackContext context) {
        currentCameraState.OnRotateCamera(context, this);
    }

    public void OnLockOn(InputAction.CallbackContext context) {
        currentCameraState.OnLockOn(context, this);
    }

    public void OnLockOnChange(InputAction.CallbackContext context) {
        currentCameraState.OnLockOnChange(context, this);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    /* #endregion */
}
