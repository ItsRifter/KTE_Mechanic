using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    //Makes the door unlocked on start
    [SerializeField]
    bool startUnlocked = true;

    //Makes the door start opened on start
    [SerializeField]
    bool startOpened = false;

    bool isUnlocked;

    enum InteractMethod
    {
        Handle, //Door can be interacted with a handle
        Logic, //Door relies on other interactions like buttons
        Powered //Door needs power to open
    }

    [SerializeField]
    InteractMethod interactingMethod;

    //The status of the door whether its opening, opened vice versa
    enum DoorStatus
    {
        Opening,
        Opened,
        Closing,
        Closed
    }

    DoorStatus doorStatus;

    // Start is called before the first frame update
    void Start()
    {
        isUnlocked = startUnlocked;

        doorStatus = startOpened ? DoorStatus.Opened : DoorStatus.Closed;
    }

    Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        if(doorStatus == DoorStatus.Opening)
        {

        }
        else if (doorStatus == DoorStatus.Closing)
        {

        }
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
    public void Open()
    {
        if (doorStatus == DoorStatus.Opening || !CanInteract()) return;

        targetRotation = Quaternion.Euler(0, 90, 0);
        doorStatus = DoorStatus.Opening;
    }

    void FinishOpening()
    {
        doorStatus = DoorStatus.Opened;
    }    

    /// <summary>
    /// Closes the door
    /// </summary>
    public void Close()
    {
        if (doorStatus == DoorStatus.Closed || !CanInteract()) return;
        
        targetRotation = Quaternion.Euler(0, -90, 0);
        doorStatus = DoorStatus.Closing;
    }

    void FinishClosing()
    {
        doorStatus = DoorStatus.Closed;
    }

    /// <summary>
    /// Toggles the door status to open or close
    /// </summary>
    public void Toggle()
    {
        if (!CanInteract()) return;

        if (doorStatus == DoorStatus.Opened) Open();
        else if (doorStatus == DoorStatus.Closed) Close();
    }

    /// <summary>
    /// Can this door be interacted
    /// </summary>
    /// <returns>The door can be opened or closed</returns>
    bool CanInteract() => isUnlocked;
}
