using System.Collections;
using UnityEngine;

public class EyeFOV : MonoBehaviour
{
    [HideInInspector]
    public float radius;

    [HideInInspector]
    public float angleFOV;

    //The player to target
    [HideInInspector]
    public GameObject refPlayer;

    [SerializeField]
    LayerMask targetMask;

    [SerializeField]
    LayerMask ignoreMasks;

    [HideInInspector]
    public bool canSeePlayer;

    NPCNav navAgent;
    
    void Start()
    {
        refPlayer = SurvivalManager.GetPlayerReference();
        navAgent = GetComponent<NPCNav>();

        //Start searching coroutine
        StartCoroutine(SearchRoutine());
    }

    //Handles searching
    IEnumerator SearchRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);

        //TODO: Check and functions if player was found
        while (!canSeePlayer)
        {
            yield return delay;
            FOVCheck();

            if (canSeePlayer)
            {
                navAgent.MoveToTarget(refPlayer.transform.position);
                break;
            }
        }

        if (canSeePlayer)
        {
            navAgent.MoveToTarget(refPlayer.transform.position);

            if(!refPlayer.GetComponent<PlayerStatus>().inHiding)
                FOVCheck();

            yield return delay;
        }

        /*while(canSeePlayer)
        {
            yield return delay;

            navAgent.MoveToTarget(refPlayer.transform.position);
            FOVCheck();
        }*/
    }

    void FOVCheck()
    {
        Collider[] sightChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (sightChecks.Length != 0)
        {
            Transform target = sightChecks[0].transform;
            Vector3 targetDir = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, targetDir) < angleFOV / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                //Check if we can or can't see the player, set bool accordingly to result
                if (!Physics.Raycast(transform.position, targetDir, distToTarget, ignoreMasks))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
        }
        //If the NPC did see the player but lost sight, set bool to false
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
