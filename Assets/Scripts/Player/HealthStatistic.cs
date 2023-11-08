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

    // Start is called before the first frame update
    void Start()
    {
        //Set health to starting value
        CurHealth = startHealth;
    }

    /// <summary>
    /// Hurt the player.
    /// </summary>
    /// <param name="dmg">The amount of damage to deal</param>
    public void TakeDamage(float dmg)
    {
        CurHealth -= dmg;

        Debug.Log($"Took {dmg} | Health {CurHealth}");

        //When the player has no health, do dying functions
        if (CurHealth <= 0)
            OnKilled();
    }

    public void SetHealth(float setHP)
    {
        CurHealth = setHP;

        if (CurHealth <= 0)
            OnKilled();
    }

    //Performs death functions
    void OnKilled()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
