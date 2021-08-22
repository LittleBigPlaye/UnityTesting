using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{

    public InputActionAsset inputActionAsset;
    public float rotationPower = 3f;
    public float rotationLerp = .5f;
    public Transform cameraFollowTransform;
    public Transform playerTransform;
    private InputActionMap inputActionMap;
    private InputAction cameraMoveAction;
    private Vector2 cameraMovement;

    private Quaternion nextPlayerRotation;

    private void Awake()
    {
        inputActionMap = inputActionAsset.FindActionMap("Player");
        cameraMoveAction = inputActionMap.FindAction("Rotate Camera");

        cameraMoveAction.performed += MoveCamera;
        cameraMoveAction.canceled += MoveCamera;
    }

    private void MoveCamera(InputAction.CallbackContext context)
    {
        cameraMovement = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //rotate camera horizontally
        cameraFollowTransform.rotation *= Quaternion.AngleAxis(cameraMovement.x * rotationPower, Vector3.up);

        //rotate camera vertically
        cameraFollowTransform.rotation *= Quaternion.AngleAxis(cameraMovement.y * -rotationPower, Vector3.right);

        Vector3 angles = cameraFollowTransform.localEulerAngles;
        angles.z = 0;

        float angle = cameraFollowTransform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        cameraFollowTransform.transform.localEulerAngles = angles;

        nextPlayerRotation = Quaternion.Lerp(cameraFollowTransform.rotation, nextPlayerRotation, Time.deltaTime * rotationLerp);
        playerTransform.rotation = Quaternion.Euler(0, cameraFollowTransform.rotation.eulerAngles.y, 0);


    }

    private void OnEnable()
    {
        cameraMoveAction.Enable();
    }

    private void OnDisable()
    {
        cameraMoveAction.Disable();
    }
}
