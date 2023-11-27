using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SentientSlime : NPCNav
{
    public override IEnumerator DoUniqueObjectBehaving()
    {
        var transport = lastTargetObject.GetComponentInParent<NPCTransportation>();

        if (transport != null)
        {
            transport.TransportNPC(gameObject);
        }

        lastTargetObject = null;

        yield return null;
    }

    public override bool DoThinkingTarget()
    {
        if(Random.Range(0, 5) >= 3)
        {
            Debug.Log("Thought");
            GameObject vent = FindNearestVent();

            GoToTargetPoint(vent.transform.GetChild(1).gameObject);

            return true;
        }

        return false;
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
