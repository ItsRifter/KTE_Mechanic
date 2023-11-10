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

    [HideInInspector]
    public float curStamina;

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
        //Console is enabled, stop here
        if (GameConsole.consoleInstance.showConsole) return;

        //If controls are disabled, stop here
        if (!HealthStatistic.allowControls) return;

        //Do any move inputs from the player
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!IsGrounded())
            ApplyGravity();

        RegenStamina();

        AnalyticTracker.instance.UpdateDistanceTravelled(transform.position);

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

    //Returns if the player is grounded
    bool IsGrounded()
    {
        Vector3 point = GameObject.Find("PlayerGround").transform.position;
        return Physics.Raycast(point, Vector3.down, 0.1f, LayerMask.GetMask("Walkable"));
    }

    void ApplyGravity() => transform.position += Vector3.down * (Time.deltaTime * 9.81f);
}
