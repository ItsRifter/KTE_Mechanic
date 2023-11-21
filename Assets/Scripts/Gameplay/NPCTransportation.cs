using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCTransportation : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(SurvivalManager.IsObjectNPC(other.gameObject))
        {
            if (IsChasingPlayer(other.gameObject)) return;

            var behaveType = other.gameObject.GetComponent<NPCTypeStats>().behaviourType;

            if(behaveType == NPCTypeStats.BrainBehaviour.Phase_Shifting)
                TransportNPC(other.gameObject);
        }
    }

    void TransportNPC(GameObject gameObj)
    {
        var points = GetTransportPoints(true);
        int maxPoints = points.Count() - 1;

        var exitPoint = points[Random.Range(0, maxPoints)].transform.GetChild(0);

        gameObj.transform.position = exitPoint.transform.position;
        gameObj.transform.rotation = exitPoint.transform.localRotation;
    }

    /// <summary>
    /// Check if the NPC is pursuring the player
    /// </summary>
    /// <param name="gameObj">The NPC</param>
    /// <returns>Player is being chased by this NPC</returns>
    bool IsChasingPlayer(GameObject gameObj) => gameObj.GetComponent<EyeFOV>().canSeePlayer;

    List<GameObject> GetTransportPoints(bool excludeSelf = false)
    {
        List<GameObject> tpPoints = GameObject.FindGameObjectsWithTag("TelePointNPC").ToList();

        if (excludeSelf)
            tpPoints.Remove(gameObject);

        return tpPoints;
    }
}
