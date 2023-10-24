using UnityEngine;
using UnityEngine.AI;

public class NPCNav : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent navAgent;
    Vector3 targetPosition;

    //NPCTypeStats stats;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
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
    public void MoveToTarget(Vector3 pos)
    {
        targetPosition = pos;
        navAgent.SetDestination(targetPosition);
    }

    public void MoveToLastSeen(Vector3 lastPos)
    {
        //TODO: change state to hunting after reaching last seen position
        targetPosition = lastPos;
        navAgent.SetDestination(targetPosition);
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
