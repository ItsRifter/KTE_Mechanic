using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SentientSlime : NPCNav
{
    [HideInInspector]
    public bool movementSlowed = false;

    public override void DoUniqueObjectBehaving()
    {
        var transport = lastTargetObject.GetComponentInParent<NPCTransportation>();

        if (transport != null)
        {
            transport.TransportNPC(gameObject);
        }

        lastTargetObject = null;
    }

    public IEnumerator TemporaryMovementSlowdown()
    {
        movementSlowed = true;

        yield return new WaitForSeconds(3.25f);

        movementSlowed = false;
    }

    public override bool DoThinkingTarget()
    {
        if(Random.Range(0, 7) >= 4)
        {
            GameObject vent = FindNearestVent();

            GoToTargetPoint(vent.transform.GetChild(1).gameObject);

            return true;
        }

        return false;
    }

    public override void SimulateNPC()
    {
        float orgSpeed = GetComponent<NPCTypeStats>().baseSpeed;
        navAgent.speed = orgSpeed / (movementSlowed ? 5.0f : 1.0f);

        base.SimulateNPC();
    }

    GameObject FindNearestVent()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("TransportNPC");

        GameObject nearest = objects[0];
        float closeDist = Vector3.Distance(gameObject.transform.position, nearest.transform.position);

        foreach (GameObject gameObj in objects)
        {
            float distCheck = Vector3.Distance(gameObject.transform.position, gameObj.transform.position);

            if(distCheck < closeDist)
            {
                nearest = gameObj;
                closeDist = distCheck;
            }
        }

        return nearest;
    }
}
