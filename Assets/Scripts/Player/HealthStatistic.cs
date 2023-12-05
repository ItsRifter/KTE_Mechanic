using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthStatistic : MonoBehaviour
{
    //The starting health for when the player starts
    [SerializeField]
    float startHealth = 100;

    //How much health does the player currently have
    [HideInInspector]
    public float CurHealth { get; private set; }

    [HideInInspector]
    public bool onGodMode;

    //Allows player input
    public static bool allowControls;

    GameObject lastAttacker;

    // Start is called before the first frame update
    void Start()
    {
        allowControls = false;

        //Set health to starting value
        CurHealth = startHealth;
    }

    /// <summary>
    /// Hurt the player.
    /// </summary>
    /// <param name="dmg">The amount of damage to deal</param>
    public void TakeDamage(float dmg, GameObject attacker = null)
    {
        if (onGodMode) return;

        CurHealth -= dmg;
        lastAttacker = attacker;

        //Debug.Log($"Took {dmg} | Health {CurHealth}");

        //When the player has no health, do dying functions
        if (CurHealth <= 0)
            OnKilled();
    }

    public void SetHealth(float setHP)
    {
        CurHealth = setHP;

        //Player has less or equal to 0 health after this set, do death functions
        if (CurHealth <= 0 && !onGodMode)
            OnKilled();
    }

    //Performs death functions
    void OnKilled()
    {
        //Show statistics to screen
        MenuScreen.instance.ShowStatistics();

        allowControls = false;

        //Get the killers name
        string killerName = lastAttacker.GetComponent<NPCTypeStats>().npcName ?? "Nothing";
        AnalyticTracker.instance.StoreDataInAnalytics(killerName);

        //Makes the camera a bit cinematic with death in local transform
        var camera = Camera.main;
        camera.transform.localPosition = new Vector3(0, -0.75f, 0.11f);
        camera.transform.localRotation = new Quaternion(0, 0, -0.21f, 0.97f);
    }
}
