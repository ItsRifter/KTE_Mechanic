using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brainless : NPCNav
{

    /// <summary>
    /// Coroutine for when the NPC is bashing the door
    /// </summary>
    public IEnumerator BashDoor(DoorInteraction door)
    {
        navPaused = true;
        int bashes = 0;

        do
        {
            //Interrupt task when player is spotted or door just opened
            if (eyeFOV.canSeePlayer || door.doorStatus == DoorInteraction.DoorStatus.Opened)
                yield return null;

            bashes += 1;
            yield return new WaitForSeconds(2.25f);
        }
        while (bashes < 3);

        //Force the door open and put a interaction cooldown
        door.Open(true);
        door.timeUntilUsable = 6.5f;

        navPaused = false;

        yield return null;
    }
}
