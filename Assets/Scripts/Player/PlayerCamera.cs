using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    float mouseSens = 925f;

    Transform playerBody;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        //Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = 0f;
        playerBody = transform.parent.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        DoCameraLooking();
        HandleInteracting();
    }

    //Handles camera looking with mouse input
    void DoCameraLooking()
    {
        //get mouse inputs, times by deltatime and mouse sensitivity
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens;

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
