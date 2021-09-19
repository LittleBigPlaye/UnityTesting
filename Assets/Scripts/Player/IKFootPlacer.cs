using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKFootPlacer : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float distanceToGround = 1f;
    [SerializeField] LayerMask groundLayerMask;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {

        float ikLeftFoot = animator.GetFloat("IKLeftFootWeight");
        float ikRightFoot = animator.GetFloat("IKLeftFootWeight");

        if (ikLeftFoot > 0f)
        {
            //Set Weights for Left Foot
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikLeftFoot);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikLeftFoot);
            UpdateIKFootLocRot(AvatarIKGoal.LeftFoot);
        }

        if (ikRightFoot > 0f)
        {
            //Set Weights for Right Foot
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikRightFoot);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikRightFoot);
            UpdateIKFootLocRot(AvatarIKGoal.RightFoot);
        }
    }

    private void UpdateIKFootLocRot(AvatarIKGoal goal)
    {
        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(goal) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distanceToGround + 2f, groundLayerMask))
        {
            Vector3 targetFootPosition = hit.point;
            targetFootPosition.y += distanceToGround;

            animator.SetIKPosition(goal, targetFootPosition);
            animator.SetIKRotation(goal, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }
}
