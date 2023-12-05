using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWalls : MonoBehaviour
{
    public void Break()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (SurvivalManager.IsObjectNPC(other.gameObject))
        {
            //See if the NPC is an aggressive type (or in this case a Brute)
            if(other.gameObject.GetComponent<NPCTypeStats>()
              .behaviourType == NPCTypeStats.BrainBehaviour.Aggressive)
                Break();
        }
    }
}
