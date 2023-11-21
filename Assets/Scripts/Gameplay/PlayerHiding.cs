using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    bool isPlayerInRange;
    bool isHiding;

    ModelGroups modelGroupComp;

    // Start is called before the first frame update
    void Start()
    {
        modelGroupComp = GetComponent<ModelGroups>();
        isHiding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (isHiding)
                    modelGroupComp.SetModelGroup(0);
                else
                    modelGroupComp.SetModelGroup(1);

                isHiding = !isHiding;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayerObject(other.gameObject) && !isPlayerInRange)
            isPlayerInRange = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (IsPlayerObject(other.gameObject))
        {

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayerObject(other.gameObject) && isPlayerInRange)
            isPlayerInRange = false;
    }

    bool IsPlayerObject(GameObject gameObj) =>
        gameObj == SurvivalManager.GetPlayerReference();
}
