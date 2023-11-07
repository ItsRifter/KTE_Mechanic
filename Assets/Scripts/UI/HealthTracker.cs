using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    Slider healthSlider;

    float lastValue;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();

        lastValue = GetPlayerHealth();
    }

    float GetPlayerHealth() 
        => SurvivalManager.GetPlayerReference()
        .gameObject.GetComponent<HealthStatistic>().CurHealth;

    // Update is called once per frame
    void Update()
    {
        float newHealth = GetPlayerHealth();

        if (lastValue != newHealth)
            lastValue = newHealth;
        else return;

        healthSlider.value = lastValue / 100;
    }
}
