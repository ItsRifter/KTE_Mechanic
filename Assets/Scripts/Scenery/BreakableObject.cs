using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    /// <summary>
    /// Destroys the object
    /// </summary>
    public void Break()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (SurvivalManager.IsObjectNPC(other.gameObject))
        {
            //Get behaving type
            var behaveType = other.gameObject.GetComponent<NPCTypeStats>().behaviourType;

            //If the NPC is an aggressive type
            if (behaveType == NPCTypeStats.BrainBehaviour.Aggressive)
                Break();

            //If the NPC is a phase shifter
            if (behaveType == NPCTypeStats.BrainBehaviour.Phase_Shifting)
            {
                var slime = other.gameObject.GetComponent<SentientSlime>();
                StartCoroutine(slime.TemporaryMovementSlowdown(4.5f));
            }

            //If NPC is a zombie-like creature
            if (behaveType == NPCTypeStats.BrainBehaviour.Zombie_Like)
                other.gameObject.GetComponent<Brainless>().ResetAndRelocate();
        }
    }
}
