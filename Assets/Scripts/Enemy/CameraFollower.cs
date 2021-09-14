using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 followOffset = new Vector3(0,1f,0);

    void LateUpdate()
    {
        transform.position = targetTransform.position + followOffset;        
    }
}
