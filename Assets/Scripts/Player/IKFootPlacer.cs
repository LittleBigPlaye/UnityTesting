using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class IKFootPlacer : MonoBehaviour
{
    [Range(0f,1f)]
    public float distanceToGround = 1f;
    public LayerMask groundLayerMask;
    
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        //Set Weights for Left Foot
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
        UpdateIKFootLocRot(AvatarIKGoal.LeftFoot);

        //Set Weights for Right Foot
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
        UpdateIKFootLocRot(AvatarIKGoal.RightFoot);
    }

    private void UpdateIKFootLocRot(AvatarIKGoal goal) {
        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(goal) + Vector3.up, Vector3.down);
        if(Physics.Raycast(ray, out hit, distanceToGround +2f, groundLayerMask)) {
            Vector3 targetFootPosition = hit.point;
            targetFootPosition.y += distanceToGround;

            animator.SetIKPosition(goal, targetFootPosition);
            animator.SetIKRotation(goal, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }
}
