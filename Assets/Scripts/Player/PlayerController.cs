using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public Transform playerModelTransform;
    public float movementSpeed = 5f;

    private InputActionMap actionMap;
    private InputAction moveInputAction;
    private Vector2 move;
    private CharacterController characterController;
    private AnimationController animationController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        
        actionMap = inputActionAsset.FindActionMap("Player");
        moveInputAction = actionMap.FindAction("Move");

        moveInputAction.performed += Move;
        moveInputAction.canceled += Move;

    }

    private void Move(InputAction.CallbackContext context)
    {
        PlayerState currentState = animationController.CurrentState;
        if (currentState == PlayerState.WALKING || currentState == PlayerState.IDLE)
        {
            move = context.ReadValue<Vector2>();

            if (move != Vector2.zero)
            {
                animationController.Move(true, move);
            }
            else
            {
                animationController.Move(false, Vector2.zero);
            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 movement = playerModelTransform.forward * move.y + playerModelTransform.right * move.x;

        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {

    }

    private void OnEnable()
    {
        moveInputAction.Enable();
    }

    private void OnDisable()
    {
        moveInputAction.Disable();
    }
}
