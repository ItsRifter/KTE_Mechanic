using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField]
    AudioClip doorBashSound;

    //Makes the door unlocked on start
    [SerializeField]
    bool startUnlocked = true;

    //Makes the door start opened on start
    [SerializeField]
    bool startOpened = false;

    bool isUnlocked;

    Quaternion initRot;

    enum InteractMethod
    {
        Handle, //Door can be interacted with a handle
        Logic, //Door relies on other interactions like buttons
        Powered //Door needs power to open
    }

    [SerializeField]
    InteractMethod interactingMethod;

    //The status of the door whether its opening, opened vice versa

    public enum DoorStatus
    {
        Opening,
        Opened,
        Closing,
        Closed
    }

    [HideInInspector]
    public DoorStatus doorStatus;

    //Stops overusing door after interaction to spamming
    [HideInInspector]
    public float timeUntilUsable;

    // Start is called before the first frame update
    void Start()
    {
        isUnlocked = startUnlocked;

        initRot = transform.rotation;

        if (startOpened) Open(true);
        else Close(true);
    }

    void Update()
    {
        if (timeUntilUsable > 0)
            timeUntilUsable -= 1.0f * Time.deltaTime;
    }

    /// <summary>
    /// Unlocks the door
    /// </summary>
    public void Unlock()
    {
        isUnlocked = true;
    }

    /// <summary>
    /// Locks the door
    /// </summary>
    public void Lock()
    {
        isUnlocked = false;
    }    

    /// <summary>
    /// Opens the door
    /// </summary>
    public void Open(bool forced = false)
    {
        if(!forced)
            if (doorStatus == DoorStatus.Opened || !CanInteract()) return;

        var parentTranform = GetDoorHinge();
        parentTranform.Rotate(0, -90, 0);

        doorStatus = DoorStatus.Opened;
    }

    /// <summary>
    /// Closes the door
    /// </summary>
    public void Close(bool forced = false)
    {
        if(!forced)
            if (doorStatus == DoorStatus.Closed || !CanInteract()) return;

        var parentTranform = GetDoorHinge();
        parentTranform.rotation = initRot;

        doorStatus = DoorStatus.Closed;
    }

    //Get the door hinge
    Transform GetDoorHinge() => gameObject.transform.parent.transform;

    void FinishClosing()
    {
        doorStatus = DoorStatus.Closed;
    }

    void FinishOpening()
    {
        doorStatus = DoorStatus.Opened;
    }

    /// <summary>
    /// Toggles the door status to open or close
    /// </summary>
    public void Toggle()
    {
        if (!CanInteract()) return;

        if (doorStatus == DoorStatus.Closed) Open();
        else if (doorStatus == DoorStatus.Opened) Close();

        timeUntilUsable = 1.5f;

        //StartCoroutine(DoDoorAnimation());
    }

    /// <summary>
    /// Can this door be interacted
    /// </summary>
    /// <returns>The door can be opened or closed</returns>
    bool CanInteract() => isUnlocked && timeUntilUsable <= 0.0f;
        //&& !(doorStatus == DoorStatus.Opening || doorStatus == DoorStatus.Closing);

    private void OnTriggerEnter(Collider other)
    {
        if (doorStatus == DoorStatus.Opened) return;

        //Check if the collider is a NPC
        if (SurvivalManager.IsObjectNPC(other.gameObject))
        {
            var behaveType = other.gameObject.GetComponent<NPCTypeStats>().behaviourType;

            switch (behaveType)
            {
                case NPCTypeStats.BrainBehaviour.Zombie_Like:
                    StartCoroutine(other.gameObject.GetComponent<Brainless>().BashDoor(this));
                    break;

                case NPCTypeStats.BrainBehaviour.Phase_Shifting:
                    StartCoroutine(other.gameObject.GetComponent<SentientSlime>().TemporaryMovementSlowdown(3.25f));
                    break;

                case NPCTypeStats.BrainBehaviour.Aggressive:
                    Destroy(GetParent());
                    break;
            }
        }
    }

    public void PlayBashSound()
    {
        var audio = GetComponent<AudioSource>();

        audio.clip = doorBashSound;
        audio.Play();
    }

    //Gets the door as a whole in the scene
    GameObject GetParent() => transform.parent.transform.parent.gameObject;

    /// <summary>
    /// When the NPC interacts with a door
    /// </summary>
    /// <param name="npc">The NPC game object</param>
    /*void NPCInteract(GameObject npc)
    {
        //Components of the NPC
        NPCTypeStats.BrainBehaviour behaviour = npc.GetComponent<NPCTypeStats>().behaviourType;
        NPCNav nav = npc.GetComponent<NPCNav>();
        EyeFOV sightFOV = npc.GetComponent<EyeFOV>();

        if(interactingMethod == InteractMethod.Handle && doorStatus == DoorStatus.Closed)
        {
            switch (behaviour)
            {
                case NPCTypeStats.BrainBehaviour.Zombie_Like:
                {
                    nav.forceStop = true;
                    StartCoroutine(BashDoor(sightFOV));
                }
                    break;

                //TODO: add behaving methods to below
                case NPCTypeStats.BrainBehaviour.Phase_Shifting: break;
                case NPCTypeStats.BrainBehaviour.Aggressive: break;
            }

            nav.forceStop = false;
        }
    }*/

    //*PARTS OF CODE BY LIAM ACADEMY*
    //Does opening/closing lerping

    /*Quaternion targetRotation;

    IEnumerator DoDoorAnimation()
    {
        Quaternion hinge = gameObject.transform.rotation.normalized;

        gameObject.transform.rotation = Quaternion.Slerp(hinge, targetRotation, Time.deltaTime * animationSpeed);

        Debug.Log(gameObject.transform.rotation);
        Debug.Log(targetRotation);

        //Target rotation is almost equal to value, finish up and return true
        if (Quaternion.Angle(hinge, targetRotation) < 3.0f)
        {
            //hinge = targetRotation;
            
            //If the door is opening, set to opened
            //else if closing, set to closed
            if (doorStatus == DoorStatus.Opening) doorStatus = DoorStatus.Opened;
            else if (doorStatus == DoorStatus.Closing) doorStatus = DoorStatus.Closed;

            return true;
        }

        //Animation hasn't finished yet
        return false;
    }*/
}
