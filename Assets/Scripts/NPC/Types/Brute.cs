using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brute : NPCNav
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
    }
}
