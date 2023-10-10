using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //The speed of the movement
    [SerializeField]
    float speed = 25.0f;

    //The character controller for any movements
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        //Get the character controller component
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Do any move inputs from the player
        HandleMovement();
    }

    void HandleMovement()
    {
        //Get both inputs
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //If the player is not moving, stop here
        if (moveX == 0 && moveZ == 0) return;

        //Make the player move based on current position and camera forward
        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        controller.Move(movement * speed * Time.deltaTime);
    }
}
