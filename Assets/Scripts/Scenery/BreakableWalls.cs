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
            Break();
    }
}
