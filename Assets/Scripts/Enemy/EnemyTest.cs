using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    CHASE,
    SEARCH,
    RETURN,
    DEATH
}
public class EnemyTest : MonoBehaviour
{
    public float movementSpeed = 5f;
    
    private Animator animator;
    private Transform player;
    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private Vector3 initialPosition;

    private EnemyState currentState = EnemyState.IDLE;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        animator = GetComponentInChildren<Animator>();
        initialPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.CHASE:
                ChasePlayer();
                break;
            case EnemyState.RETURN:
                GoToInitialPosition();
                break;
        }
        animator.SetFloat("movementSpeed", agent.velocity.magnitude / movementSpeed);
    }

    private void ChasePlayer()
    {
        
        agent.SetDestination(player.position);
    }

    private void Patrol() {

    }

    private void GoToInitialPosition()
    {
        agent.SetDestination(initialPosition);
        if(agent.remainingDistance == 0) {
            currentState = EnemyState.IDLE;
            animator.SetBool("isMoving", false);
        }
    }

    IEnumerator ReturnToIdle() {
        yield return new WaitForSeconds(5f);
        currentState = EnemyState.RETURN;
    }

    public void StopChasingPlayer() {
        if(currentState == EnemyState.CHASE) {
            StartCoroutine(ReturnToIdle());
        }
    }

    public void StartChasingPlayer() {
        currentState = EnemyState.CHASE;
            animator.SetBool("isMoving", true);
            StopCoroutine(ReturnToIdle());
    }
}
