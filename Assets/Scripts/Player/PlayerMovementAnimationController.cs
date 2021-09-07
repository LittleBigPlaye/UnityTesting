using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    public void TriggerRoll()
    {
        animator.SetTrigger("roll");
    }

    public void EndRoll()
    {
        animator.ResetTrigger("roll");
        playerController.IsRolling = false;
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("dodge");
    }

    public void EndDodge()
    {
        animator.ResetTrigger("dodge");
        playerController.IsDodging = false;
    }

    public void EndFall()
    {
        playerController.IsFalling = false;
    }
}
