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

    //How fast the door moves on open or close
    [SerializeField]
    float animationSpeed = 1.0f;

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

    //*PARTS OF CODE BY LIAM ACADEMY
    //Does opening/closing lerping
    IEnumerator DoDoorAnimation()
    {
        Quaternion curRotation = transform.rotation;
        Quaternion endRotation;

        if(doorStatus == DoorStatus.Opening) 
        {

        }

        return null;
        /*Quaternion hinge = gameObject.transform.rotation.normalized;

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
        return false;*/
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

        Debug.Log("Door doing stuff");

        if (doorStatus == DoorStatus.Closed) Open();
        else if (doorStatus == DoorStatus.Opened) Close();

        StartCoroutine(DoDoorAnimation());
    }

    /// <summary>
    /// Can this door be interacted
    /// </summary>
    /// <returns>The door can be opened or closed</returns>
    bool CanInteract() => isUnlocked && !(doorStatus == DoorStatus.Opening || doorStatus == DoorStatus.Closing);
}
