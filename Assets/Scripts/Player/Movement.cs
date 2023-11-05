using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //The speed of the movement
    [SerializeField]
    float defaultSpeed = 3.0f;

    [SerializeField]
    float runSpeed = 4.5f;

    const float stamina = 10.0f;
    float curStamina;

    //The character controller for any movements
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        //Get the character controller component
        controller = GetComponent<CharacterController>();
        curStamina = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        //Do any move inputs from the player
        HandleMovement();
    }

    void HandleMovement()
    {
        RegenStamina();

        //Get both inputs
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //If the player is not moving, stop here
        if (moveX == 0 && moveZ == 0) return;

        //Make the player move based on current position and camera forward
        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        controller.Move(movement * GetSpeed() * Time.deltaTime);
    }

    float GetSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && curStamina > 0.0f)
        {
            curStamina -= 2f * Time.deltaTime;
            curStamina = Mathf.Clamp(curStamina, 0, stamina);

            return runSpeed;
        }

        return defaultSpeed;
    }

    void RegenStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift) || curStamina == stamina) return;

        curStamina += 0.5f * Time.deltaTime;
        curStamina = Mathf.Clamp(curStamina, 0, stamina);
    }
}
