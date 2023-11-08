using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaTracker : MonoBehaviour
{
    Slider staminaSlider;

    float lastValue;

    // Start is called before the first frame update
    void Start()
    {
        staminaSlider = GetComponent<Slider>();

        lastValue = GetPlayerStamina();
    }

    float GetPlayerStamina()
        => SurvivalManager.GetPlayerReference()
        .gameObject.GetComponent<Movement>().curStamina;

    // Update is called once per frame
    void Update()
    {
        float newStamina = GetPlayerStamina();

        if (lastValue != newStamina)
            lastValue = newStamina;
        else return;

        staminaSlider.value = lastValue / 10;
    }
}
