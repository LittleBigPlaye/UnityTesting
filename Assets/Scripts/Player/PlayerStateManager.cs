using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : CharacterStateManager
{
    [Header("Movement")]
    public float walkingSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float playerRotationSpeed = 5f;

    public float sprintDelay = 1f;
    public float rollSpeed = 3f;
    public float dodgeSpeed = 2f;
    public float hardLandingDelay = 1f;
    [SerializeField] private Vector3 gravity = Physics.gravity;

    [Header("Stamina")]
    public float sprintStamina = 5f;
    public float dodgeStamina = 20f;
    public float rollStamina = 25f;
    public float lightAttackStamina = 30f;
    public float heavyAttackStamina = 50f;
    public float blockStamina = 40f;

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

    public CharacterController CharacterController { get; private set;}

    private bool isInvincible = false;
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }


    /* #region Player States */
    private PlayerBaseState currentState;
    public readonly PlayerIdleState idleState = new PlayerIdleState();
    public readonly PlayerWalkingState walkingState = new PlayerWalkingState();

    public readonly PlayerDodgeState dodgeState = new PlayerDodgeState();
    public readonly PlayerSprintState sprintState = new PlayerSprintState();
    public readonly PlayerRollState rollState = new PlayerRollState();

    public readonly PlayerFallingState fallingState = new PlayerFallingState();
    public readonly PlayerHardLandingState hardLandingState = new PlayerHardLandingState();

    public readonly PlayerHealingState healingState = new PlayerHealingState();
    public readonly PlayerHitState hitState = new PlayerHitState();
    public readonly PlayerDeathState deathState = new PlayerDeathState();

    public readonly PlayerLightAttackState lightAttackState = new PlayerLightAttackState();
    public readonly PlayerHeavyAttackState heavyAttackState = new PlayerHeavyAttackState();
    /* #endregion */

    /* #region Camera States */
    private PlayerCameraBaseState currentCameraState;
    public PlayerCameraFreeRoamState freeRoamCameraState = new PlayerCameraFreeRoamState();
    public PlayerCameraLockOnState lockOnCameraState = new PlayerCameraLockOnState();
    /* #endregion */

    private Vector2 movementInput = Vector2.zero;
    public Vector2 MovementInput { get => movementInput; }

    private Transform lockOnTarget = null;
    public Transform LockOnTarget { get => lockOnTarget; set => lockOnTarget = value; }

    public StaminaController StaminaController { get; private set;}
    public HealthController HealthController { get; private set;}
    public InventoryController InventoryController { get; private set; }
    public CombatController CombatController {get; private set;}

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CharacterController = GetComponentInParent<CharacterController>();
        StaminaController = GetComponent<StaminaController>();
        HealthController = GetComponent<HealthController>();
        InventoryController = GetComponent<InventoryController>();
        CombatController = GetComponent<CombatController>();

        InitializeInputs();

        currentState = idleState;
        currentState.EnterState(this, null);

        currentCameraState = freeRoamCameraState;
        currentCameraState.EnterState(this);
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

        input.Player.UseItem.performed += OnUseItem;

        input.Player.LightAttack.performed += OnLightAttack;
        input.Player.HeavyAttack.started += OnHeavyAttack;
    }

    private void Update()
    {
        currentState.UpdateState(this);
        currentCameraState.UpdateState(this);
    }

    public void Move(Vector3 movementDirection)
    {
        if (CharacterController.isGrounded)
        {
            movementDirection.y -= .5f;
        }
        else
        {
            movementDirection += gravity;
        }
        CharacterController.Move(movementDirection * Time.deltaTime);
    }

    public void SwitchState(PlayerBaseState nextState)
    {
        PlayerBaseState previousState = currentState;
        currentState.ExitState(this);
        currentState = nextState;
        currentState.EnterState(this, previousState);
    }

    public void SwitchState(PlayerCameraBaseState state)
    {
        currentCameraState = state;
        currentCameraState.EnterState(this);
    }

    public override void EndState()
    {
        currentState.EndStateByAnimation(this);
    }

    public void OnHeal()
    {
        if (InventoryController.CurrentNumberOfPotions > 0)
        {
            HealthController.CurrentHealth += InventoryController.PotionHealthRestoreValue;
            InventoryController.CurrentNumberOfPotions -= 1;
        }
    }

    public override void GetHit(float damage)
    {
        if (!isInvincible)
        {
            HealthController.CurrentHealth -= damage;
            currentState.GetHit(this);
        }
    }

    #region inputs
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

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        currentCameraState.OnRotateCamera(context, this);
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        currentCameraState.OnLockOn(context, this);
    }

    public void OnLockOnChange(InputAction.CallbackContext context)
    {
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
    #endregion
}
