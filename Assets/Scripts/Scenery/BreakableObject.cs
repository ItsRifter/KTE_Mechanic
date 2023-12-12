using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public void Break()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (SurvivalManager.IsObjectNPC(other.gameObject))
        {
            var behaveType = other.gameObject.GetComponent<NPCTypeStats>().behaviourType;

            //See if the NPC is an aggressive type (or in this case a Brute)
            if (behaveType == NPCTypeStats.BrainBehaviour.Aggressive)
                Break();

            if (behaveType == NPCTypeStats.BrainBehaviour.Phase_Shifting)
            {
                var slime = other.gameObject.GetComponent<SentientSlime>();
                StartCoroutine(slime.TemporaryMovementSlowdown(4.5f));
            }
        }
    }
}
