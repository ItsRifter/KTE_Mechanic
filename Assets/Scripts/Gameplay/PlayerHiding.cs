using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    bool isPlayerInRange;

    [HideInInspector]
    bool isBroken = false;

    ModelGroups modelGroupComp;
    GameObject playerRef;

    void Start()
    {
        modelGroupComp = GetComponent<ModelGroups>();
        playerRef = SurvivalManager.GetPlayerReference();
    }

    void Update()
    {
        //Locker is broken and cannot be used
        if (isBroken) return;

        if (isPlayerInRange)
        {
            bool isHiding = playerRef.GetComponent<PlayerStatus>().inHiding;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isHiding)
                    modelGroupComp.SetModelGroup(0);
                else
                    modelGroupComp.SetModelGroup(1);

                playerRef.GetComponent<PlayerStatus>().inHiding = !isHiding;
            }
        }
    }

    //Breaks the locker rendering it ineffective
    public void Break()
    {
        modelGroupComp.SetModelGroup(2);

        playerRef.GetComponent<PlayerStatus>().inHiding = false;
        isPlayerInRange = false;

        isBroken = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayerObject(other.gameObject) && !isPlayerInRange)
            isPlayerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayerObject(other.gameObject) && isPlayerInRange)
            isPlayerInRange = false;
    }

    public bool IsPlayerHiding()
    {
        return playerRef.GetComponent<PlayerStatus>().inHiding;
    }

    public void ForceOutOfHiding()
    {
        playerRef.GetComponent<PlayerStatus>().inHiding = false;
        modelGroupComp.SetModelGroup(0);
    }

    bool IsPlayerObject(GameObject gameObj) =>
        gameObj == SurvivalManager.GetPlayerReference();
}
