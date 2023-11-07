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
        .gameObject.GetComponent<Movement>().CurStamina;

    // Update is called once per frame
    void Update()
    {
        float newHealth = GetPlayerStamina();

        if (lastValue != newHealth)
            lastValue = newHealth;
        else return;

        staminaSlider.value = lastValue * 10;
    }
}
