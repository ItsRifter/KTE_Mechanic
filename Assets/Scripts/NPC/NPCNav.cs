using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCNav : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent navAgent;
    Vector3 targetPosition;

    bool isActive = false;

    [HideInInspector]
    public GameObject lastTargetObject;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(SetupNavigation());
    }

    IEnumerator SetupNavigation()
    {
        yield return new WaitForSeconds(0.1f);
        isActive = true;
    }

    Vector3 ApplyGravity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.1f, NavMesh.AllAreas))
            return Vector3.zero;

        return Vector3.down * Time.deltaTime;
    }

    //Makes the NPC stop moving, useful for performing states
    [HideInInspector]
    public bool forceStop;

    void Update()
    {
        if (forceStop) return;

        if (!isActive)
            //Try to put NPC on ground to prevent seeing object floating
            transform.position += ApplyGravity();
        else
        {
            if (HasReachedTarget())
            {
                Debug.Log(lastTargetObject);

                if (lastTargetObject)
                    StartCoroutine(DoUniqueObjectBehaving());

                if (GetRandomPoint(transform.position, 64.0f, out Vector3 result))
                {
                    if(!DoThinkingTarget())
                        MoveToTarget(result);
                }
            }
        }

        navAgent.isStopped = IsNearPlayer();
    }

    //Is the NPC near the player
    bool IsNearPlayer()
    {
        GameObject player = SurvivalManager.GetPlayerReference();
        if(player == null) return false;

        float radius = NPCTypeStatExtensions.GetNPCStatistics(gameObject).GetRadius() / 2.5f;

        return Vector3.Distance(transform.position, player.transform.position) <= radius;
    }

    public virtual bool DoThinkingTarget()
    {
        return false;
    }

    //Returns if the destination has been reached
    public virtual bool HasReachedTarget()
    {
        Vector3 curPos = transform.position;

        return Vector3.Distance(curPos, targetPosition) > 0.5f;
    }

    public virtual IEnumerator DoUniqueObjectBehaving()
    {
        lastTargetObject = null;
        yield return null;
    }

    /// <summary>
    /// Move towards a target point
    /// </summary>
    public virtual void GoToTargetPoint(GameObject target)
    {
        MoveToTarget(target.transform.position);
        lastTargetObject = target;
    }

    /// <summary>
    /// Decides if the NPC wishes to move to a special target point
    /// E.G: Vents, doors or lockers
    /// </summary>
    public virtual bool DecideTargetPoint(Vector3 rayHit, out GameObject targetObj)
    {
        targetObj = null;
        return false;
    }

    //Moves to the target position
    public virtual void MoveToTarget(Vector3 pos)
    {
        targetPosition = pos;
        navAgent.SetDestination(targetPosition);
    }

    public virtual void MoveToLastSeen(Vector3 lastPos)
    {
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
