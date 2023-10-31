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
    public float curHealth { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //Set health to starting value
        curHealth = startHealth;
    }

    /// <summary>
    /// Hurt the player.
    /// </summary>
    /// <param name="dmg">The amount of damage to deal</param>
    public void TakeDamage(float dmg)
    {
        curHealth -= dmg;

        Debug.Log($"Took {dmg} | Health {curHealth}");

        //When the player has no health, do dying functions
        if (curHealth <= 0)
            OnKilled();
    }

    //Performs death functions
    void OnKilled()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
