using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;

    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();
    private bool hasFoundPlayer = false;
    private EnemyTest enemyController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyTest>();
    }

    void Start()
    {
        StartCoroutine(FindTargetsWithDelay(.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        for (; ; )
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    public void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        bool foundPlayer = targetsInViewRadius.Length > 0;
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    hasFoundPlayer = true;
                    visibleTargets.Add(target);
                }
                else if (hasFoundPlayer)
                {
                    foundPlayer = false;
                }
            }
            else
            {
                foundPlayer = false;
            }
        }

        if (foundPlayer && hasFoundPlayer)
        {
            hasFoundPlayer = false;
            enemyController.StartChasingPlayer();
        }
        else
        {
            enemyController.StopChasingPlayer();
        }
    }

    public Vector3 GetDirectionFromAngle(float angle, bool isGlobalAngle)
    {
        if (!isGlobalAngle)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
