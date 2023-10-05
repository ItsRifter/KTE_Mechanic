using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNav : MonoBehaviour
{
    NavMeshAgent navAgent;
    Vector3 targetPosition;

    NPCTypeStats stats;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        TryApplyStatistics();
    }

    void TryApplyStatistics()
    {
        NPCTypeStats stats = GetComponent<NPCTypeStats>() ?? null;
        if (stats == null) return;

        navAgent.speed = stats.GetPatrolSpeed();
    }

    void Update()
    {
        if (!HasReachedTarget()) return;

        if (GetRandomPoint(transform.position, 64.0f, out Vector3 result))
            MoveToTarget(result);
    }

    //Returns if the destination has been reached
    bool HasReachedTarget()
    {
        Vector3 curPos = transform.position;
        return Vector3.Distance(curPos, targetPosition) > 0.5f;
    }

    //Moves to the target position
    void MoveToTarget(Vector3 pos)
    {
        targetPosition = pos;
        navAgent.SetDestination(targetPosition);
    }

    void MoveToLastSeen()
    {

    }

    //Finds a random point within a navmesh
    //Returns true if a point is made with position else false and Vector3.zero
    bool GetRandomPoint(Vector3 pos, float range, out Vector3 result)
    {
        Vector3 randomPos = pos + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if(NavMesh.SamplePosition(randomPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
