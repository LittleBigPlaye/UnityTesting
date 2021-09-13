using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform targetTransform;

    void LateUpdate()
    {
        transform.position = targetTransform.position;        
    }
}
