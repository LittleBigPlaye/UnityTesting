using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    WALKING

}
public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerState currentState;
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
        animator = GetComponentInChildren<Animator>();
        currentState = PlayerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(bool isMoving, Vector2 move)
    {
        if (isMoving)
        {
            currentState = PlayerState.WALKING;
        }
        animator.SetBool("isWalking", isMoving);
        animator.SetFloat("movementForwardSpeed", move.y);
        animator.SetFloat("movementSidewardSpeed", move.x);
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
