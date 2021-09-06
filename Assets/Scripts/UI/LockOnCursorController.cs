using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCursorController : MonoBehaviour
{

    public Transform camera;
    private Animator animator;

    private Transform followTarget;
    public Transform FollowTarget
    {
        get { return followTarget; }
        set
        {

            followTarget = value;
            if (followTarget != null)
            {
                animator.SetBool("isHidden", false);
            }
            else
            {
                animator.SetBool("isHidden", true);
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (followTarget != null)
        {
            transform.LookAt(camera);
            transform.position = followTarget.position;
        }
    }
}
