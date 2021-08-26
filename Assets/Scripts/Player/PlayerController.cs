using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public Transform playerModelTransform;
    public Transform camFollower;
    public float movementSpeed = 5f;
    public float maxSpeed = 7;
    public float rotationSpeed = 5f;
    //in seconds, described when the player starts to sprint
    public float sprintDelay = 1f;
    public float rollSpeed = 3f;

    private InputActionMap actionMap;
    private InputAction moveInputAction;
    private InputAction sprintInputAction;
    private Vector2 move;
    private CharacterController characterController;
    private AnimationController animationController;
    private float currentSpeed = 0;
    private bool isSprintButtonPressed = false;
    private bool isSprinting = false;
    private Coroutine sprintRoutine;
    private Vector3 rollMovement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponentInChildren<AnimationController>();

        actionMap = inputActionAsset.FindActionMap("Player");
        moveInputAction = actionMap.FindAction("Move");
        sprintInputAction = actionMap.FindAction("Sprint");
        sprintInputAction.performed += OnSprint;
        sprintInputAction.canceled += OnSprint;
        moveInputAction.performed += OnMove;
        moveInputAction.canceled += OnMove;
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (animationController.CurrentState == PlayerState.IDLE || animationController.CurrentState == PlayerState.WALKING)
        {
            bool isPressed = context.ReadValue<float>() > 0.5f;
            if (context.performed)
            {
                Debug.Log("Gedrückt");
                sprintRoutine = StartCoroutine(StartSprintCountdown());
            }
            else
            {
                StopCoroutine(sprintRoutine);
                if (isSprinting)
                {
                    Debug.Log("Keine Rolle");
                }
                else
                {
                    if (animationController.CurrentState == PlayerState.WALKING)
                    {
                        animationController.TriggerRoll();
                        //get the current camera orientation to prevent rolling in circles
                        Debug.Log("Jetzt rollen");
                    }

                }
                isSprinting = false;
            }
        }
        else
        {
            StopCoroutine(sprintRoutine);
        }
    }



    IEnumerator StartSprintCountdown()
    {
        Debug.Log("Started");
        yield return new WaitForSeconds(sprintDelay);
        isSprinting = true;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        PlayerState currentState = animationController.CurrentState;
        if (currentState == PlayerState.WALKING || currentState == PlayerState.IDLE)
        {
            move = context.ReadValue<Vector2>();
            //ensures that the player does not move faster when moving diagonally
            move = move.magnitude > 1 ? move.normalized : move;
            if (move.sqrMagnitude > 0)
            {
                animationController.CurrentState = PlayerState.WALKING;
            }
        }
        if (context.canceled)
        {
            animationController.Move(false, 0);
        }
    }

    private void Move()
    {
        Vector2 currentInput = (isSprinting) ? move.normalized : move;

        Vector3 movement = camFollower.forward * currentInput.y + camFollower.right * currentInput.x;
        //to prevent the player from going upwards manually
        movement.y = 0;
        RotatePlayer(movement);

        float targetMovementSpeed = (isSprinting) ? maxSpeed : movementSpeed;

        currentSpeed = movement.normalized.magnitude * targetMovementSpeed;
        if (move != Vector2.zero)
        {
            animationController.Move(true, CalculateSpeedPercentage(move));
        }
        else
        {
            animationController.Move(false, 0);
        }
        characterController.Move(movement * targetMovementSpeed * Time.deltaTime);
    }

    private void Roll()
    {
        Vector3 movement = playerModelTransform.forward * rollSpeed;
        //to prevent the player from going upwards manually
        movement.y = 0;
        characterController.Move(movement * Time.deltaTime);
    }

    private void RotatePlayer(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            if (Quaternion.Angle(playerModelTransform.rotation, targetRotation) > .01f)
            {
                Quaternion nextRotation = Quaternion.Lerp(playerModelTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                playerModelTransform.rotation = nextRotation;
            }
        }
    }

    //calculates the percentage of the current movement speed for the animator
    private float CalculateSpeedPercentage(Vector2 input)
    {
        float speedPercentage = 0f;
        if (isSprinting)
        {

            speedPercentage = currentSpeed * input.magnitude / movementSpeed / 2f;
        }
        else
        {
            speedPercentage = currentSpeed * input.magnitude / maxSpeed;
        }
        return speedPercentage;
    }

    void Update()
    {
        switch (animationController.CurrentState)
        {
            case PlayerState.WALKING:
                Move();
                break;
            case PlayerState.ROLLING:
                Roll();
                break;

        }
    }

    private void OnEnable()
    {
        moveInputAction.Enable();
        sprintInputAction.Enable();
    }

    private void OnDisable()
    {
        moveInputAction.Disable();
        sprintInputAction.Disable();
    }
}
