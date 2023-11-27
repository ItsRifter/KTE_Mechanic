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
        Debug.Log(other.gameObject);

        if (SurvivalManager.IsObjectNPC(other.gameObject))
            Break();
    }
}
