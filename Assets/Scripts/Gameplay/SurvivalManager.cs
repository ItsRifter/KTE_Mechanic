using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct UniqueSpawnStruct
{
    public int uniqueSpawnValue;
    public NPCSpawnpoint.NPCToSpawn spawningNPC;
}

//Preferably a game manager but handles surviving
public class SurvivalManager : MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel;

    [HideInInspector]
    public float timeSurvived = -10.0f;

    [SerializeField]
    float startTimeInterval = 25.0f;

    float curTimeInterval;

    int timesSpawnedNPC = 0;

    float intervalTimer;

    /// <summary>
    /// The moment the difficulty should be adjusted from total NPC spawns
    /// </summary>
    [SerializeField]
    int[] difficultyTweaks;

    /// <summary>
    /// The new interval time in line with difficulty tweaks
    /// </summary>
    [SerializeField]
    float[] intervalAdjusts;

    //Spawns a specific NPC based on how many have spawned previously
    //E.G: If game spawned 3 NPC's and this will spawn on the 4th, use the NPC type for that time
    [SerializeField]
    UniqueSpawnStruct[] uniqueSpawns;

    int curStage;

    int lastUniqueSpawn;

    /// <summary>
    /// Gets the player game object
    /// </summary>
    public static GameObject GetPlayerReference() 
        => GameObject.FindGameObjectWithTag("Player");

    /// <summary>
    /// Determines if the game object is an NPC
    /// </summary>
    /// <param name="gameObj">The object to check</param>
    /// <returns>The object is an NPC</returns>
    public static bool IsObjectNPC(GameObject gameObj) => gameObj.GetComponent<NPCTypeStats>() != null;

    //public static bool GetNPCBehaviour();

    //Pauses survival timer, this also pauses NPC spawning
    [HideInInspector]
    public bool pauseTimer = true;

    public static SurvivalManager instance;

    void Start()
    {
        lastUniqueSpawn = uniqueSpawns.Max(l => l.uniqueSpawnValue);

        //SetNPCSpawnTimes();
        pauseTimer = true;

        instance = this;

        curTimeInterval = startTimeInterval;
        curStage = 0;
    }

    bool canSpawn = true;

    //COMMENTED OUT FOR BETTER GAMEPLAY FLEXIBILITY
    //Makes an NPC spawn based on how many have spawned previously
    //THIS IS NOT SPAWN TIME INTERVALS
    /*void SetNPCSpawnTimes()
    {
        //BioOrganics
        spawnUniqueNPCs.Add(3, NPCSpawnpoint.NPCToSpawn.BioOrganic);
        spawnUniqueNPCs.Add(6, NPCSpawnpoint.NPCToSpawn.BioOrganic);

        //Brutes
        spawnUniqueNPCs.Add(11, NPCSpawnpoint.NPCToSpawn.Brute);
    }*/

    void Update()
    {
        //Stop here if the timer is paused
        if (pauseTimer) return;

        HandlePausing();

        //Toggles game console
        if (Input.GetKeyDown(KeyCode.F1))
            GameConsole.ToggleConsole();

        //If player is alive increase timer otherwise stop here
        if (IsPlayerAlive())
            timeSurvived += 1.0f * Time.deltaTime;
        else return;

        intervalTimer = MathF.Round(timeSurvived, 2) % curTimeInterval;

        //If a time intevral has been met..
        if (intervalTimer == 0 && canSpawn)
        {
            //Spawn a new NPC
            SpawnNPC();
            canSpawn = false;

            //If enough NPCs have spawned, increase the difficulty
            if (difficultyTweaks.Contains(timesSpawnedNPC))
            {
                //Safety check if exceeding interval adjustment array
                if (curStage > intervalAdjusts.Length) return;

                curTimeInterval = intervalAdjusts[curStage];
                curStage++;

                Debug.Log($"New time interval {curTimeInterval}");
            }
        }
        else if (Math.Round(timeSurvived, 2) % curTimeInterval == 1)
            canSpawn = true;
    }

    //Spawns a NPC at a random spawnpoint
    void SpawnNPC()
    {
        var point = NPCSpawnpoint.FindRandomSpawnpoint();

        //If a unique spawn should happen based on times an NPC spawned
        if(uniqueSpawns.Any(v => timesSpawnedNPC == v.uniqueSpawnValue))
        {
            var NPC = uniqueSpawns.Where(v => timesSpawnedNPC == v.uniqueSpawnValue)
                .First().spawningNPC;

            point.SpawnNPC(NPC);
        }
        //Otherwise continue as normal
        else
        {
            if (timesSpawnedNPC < lastUniqueSpawn)
                point.SpawnNPC(NPCSpawnpoint.NPCToSpawn.Brainless);
            else
                point.SpawnNPC(NPCSpawnpoint.NPCToSpawn.Random);
        }

        timesSpawnedNPC++;
    }

    //Returns if the player is alive
    bool IsPlayerAlive()
        => GetPlayerReference().GetComponent<HealthStatistic>().CurHealth > 0.0f;


    bool isPaused = false;
    bool lastPause;

    /// <summary>
    /// Handles pause menu
    /// </summary>
    void HandlePausing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPaused = !isPaused;

        //If "isPause" changed from last state
        if (lastPause != isPaused)
        {
            //Show or hide pause menu
            pausePanel.SetActive(isPaused);

            lastPause = isPaused;
        }

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        } 
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        HealthStatistic.allowControls = !isPaused;
    }

    /// <summary>
    /// Updates mouse sensitivity for player looking
    /// </summary>
    public void UpdateMouseSens(Slider slider)
    {
        //Set mouse sensitivity value
        PlayerCamera.playerCamera.mouseSens = MathF.Round(slider.value, 2);
        
        //Update text to display new value
        GameObject.Find("SensText").GetComponent<Text>().text
            = $"Sensitivity {PlayerCamera.playerCamera.mouseSens}";
    }

    /// <summary>
    /// Handles game quitting
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quitted");
    }
}
