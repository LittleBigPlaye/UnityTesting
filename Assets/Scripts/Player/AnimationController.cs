using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    WALKING,
    ROLLING

}
public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerState currentState;
    private bool isMoving = false;
    public PlayerState CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;
            UpdateAnimator();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = PlayerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopRolling()
    {
        currentState = isMoving ? PlayerState.WALKING : PlayerState.IDLE;
    }

    public void LockOnMove(bool isMoving, Vector2 move)
    {
        if (isMoving)
        {
            currentState = PlayerState.WALKING;
        }
        this.isMoving = isMoving;
        animator.SetBool("isWalking", isMoving);
        animator.SetFloat("movementForwardSpeed", move.y);
        animator.SetFloat("movementSidewardSpeed", move.x);
    }

    public void Move(bool isMoving, float movementSpeed) {
        if(isMoving) {
            currentState = PlayerState.WALKING;
        }
        animator.SetBool("isWalking", isMoving);
        animator.SetFloat("movementForwardSpeed", movementSpeed);
    }

    public void TriggerRoll() {
        if(currentState != PlayerState.ROLLING) {
            currentState = PlayerState.ROLLING;
            animator.SetTrigger("roll");
        }
    }

    private void UpdateAnimator()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                animator.SetTrigger("startIdle");
                break;

            case PlayerState.WALKING:
                animator.SetBool("isWalking", true);
                break;
        }
    }
}
