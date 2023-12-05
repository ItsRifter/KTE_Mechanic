using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [HideInInspector]
    public float mouseSens = 2;

    const float defaultMouseSens = 200.0f;

    Transform playerBody;

    float xRotation;

    public static PlayerCamera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = this;

        xRotation = 0f;
        playerBody = transform.parent.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Console is enabled, stop here
        if (GameConsole.consoleInstance.showConsole) return;

        //If controls are disabled, don't perform camera looking, interactions etc.
        if (!HealthStatistic.allowControls) return;

        DoCameraLooking();
        HandleInteracting();
    }

    //Handles camera looking with mouse input
    void DoCameraLooking()
    {
        //get mouse inputs, times by deltatime and mouse sensitivity
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * (defaultMouseSens * mouseSens);
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (defaultMouseSens * mouseSens);

        //Applies rotation to player and camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    //Handles interacting with objects (for now doors)
    void HandleInteracting()
    {
        DoorInteraction door = null;

        if (Input.GetKeyDown(KeyCode.E))
            door = FindDoor();

        if (door == null) return;

        door.Toggle();
    }

    //Finds a door that is within camera view
    DoorInteraction FindDoor()
    {
        Ray ray = new(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 2, LayerMask.GetMask("Interactable") ))
            return hit.collider.gameObject.GetComponent<DoorInteraction>();

        return null;
    }
}
