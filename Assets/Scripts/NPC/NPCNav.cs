using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCNav : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent navAgent;

    [HideInInspector]
    public GameObject lastTargetObject;

    [HideInInspector]
    public bool navPaused;

    [HideInInspector]
    public EyeFOV eyeFOV;

    bool isActive = false;

    Vector3 targetDest;

    //Makes the NPC stop moving, useful for performing states
    [HideInInspector]
    public bool forceStop;

    //Should the navigation recalculate the path
    //Useful if the NPC is stuck
    bool shouldRecalculate;

    float timeToRetry;

    Vector3 lastPos;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        eyeFOV = GetComponent<EyeFOV>();

        StartCoroutine(SetupNavigation());
    }

    IEnumerator SetupNavigation()
    {
        yield return new WaitForSeconds(0.1f);

        lastPos = transform.position;
        isActive = true;
    }

    Vector3 ApplyGravity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.1f, NavMesh.AllAreas))
            return Vector3.zero;

        return Vector3.down * Time.deltaTime;
    }

    void Start()
    {
        lastTargetObject = null;

        timeToRetry = 3.0f;
        shouldRecalculate = false;
    }

    void Update()
    {
        bool isChasing = eyeFOV.canSeePlayer;

        navAgent.isStopped = navPaused || IsNearPlayer();

        if (!isChasing)
            SimulateNPC();
        else
            GoToTargetPoint(SurvivalManager.GetPlayerReference().gameObject);
    }

    public virtual void SimulateNPC()
    {
        if (forceStop) return;

        if (!isActive)
            //Try to put NPC on ground to prevent seeing object floating
            transform.position += ApplyGravity();
        else
        {
            //Check if the NPC isn't stuck
            if (Vector3.Distance(transform.position, lastPos) > 1.0f)
            {
                lastPos = transform.position;
                timeToRetry = 3.0f;
            }
            else
            {
                timeToRetry -= 1.0f * Time.deltaTime;

                //If stuck, try to recalculate nav destination
                if(timeToRetry < 0.0f)
                {
                    timeToRetry = 3.0f;
                    shouldRecalculate = true;
                }
            }

            if (HasReachedTarget() || shouldRecalculate)
            {
                if (GetRandomPoint(transform.position, 64.0f, out Vector3 result))
                {
                    if (!DoThinkingTarget())
                        MoveToTarget(result);
                }

                if (lastTargetObject != null && HasReachedTarget(lastTargetObject))
                    DoUniqueObjectBehaving();
            }
        }
    }

    //Is the NPC near the player
    public bool IsNearPlayer()
    {
        GameObject player = SurvivalManager.GetPlayerReference();
        if(player == null) return false;

        Vector3 curPos = gameObject.transform.position;

        return Vector3.Distance(curPos, player.transform.position) <= 1.3f;
    }

    public virtual bool DoThinkingTarget()
    {
        return false;
    }

    //Returns if the destination has been reached
    public virtual bool HasReachedTarget()
    {
        Vector3 curPos = transform.position;

        return Vector3.Distance(curPos, targetDest) > 1.0f;
    }

    public virtual bool HasReachedTarget(GameObject target)
    {
        Vector3 curPos = transform.position;
        Vector3 targetPos = target.transform.position;

        return Vector3.Distance(curPos, targetPos) < 1.25f;
    }

    public virtual void DoUniqueObjectBehaving()
    {
        lastTargetObject = null;
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
        targetDest = pos;
        navAgent.SetDestination(pos);
    }

    public virtual void MoveToLastSeen(Vector3 lastPos)
    {
        targetDest = lastPos;
        navAgent.SetDestination(lastPos);
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
