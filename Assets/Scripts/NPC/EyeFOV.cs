using System;
using System.Collections;
using UnityEngine;

public class EyeFOV : MonoBehaviour
{
    public float radius { get; private set; }
    public float angleFOV { get; private set; }

    [HideInInspector]
    public GameObject refPlayer;

    [SerializeField]
    LayerMask targetMask;

    [SerializeField]
    LayerMask obstructMasks;

    [HideInInspector]
    public bool canSeePlayer;

    NPCTypeStats stats;

    // Start is called before the first frame update
    void Start()
    {
        refPlayer = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<NPCTypeStats>();

        if (stats != null)
            SetUpStatistics();

        StartCoroutine(SearchRoutine());
    }

    void SetUpStatistics()
    {
        radius = stats.GetRadius();
        angleFOV = stats.GetFOV();
    }

    IEnumerator SearchRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return delay;
            FOVCheck();
        }
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

                if (!Physics.Raycast(transform.position, targetDir, distToTarget, obstructMasks))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
