using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Preferably a game manager but handles surviving
public class SurvivalManager : MonoBehaviour
{
    float timeSurvived = -10.0f;

    const float startTimeInterval = 25.0f;

    float curTimeInterval;

    int timesSpawnedNPC = 0;

    int[] difficultyTweaks = new int[] { 4, 9, 16, 20 };
    float[] intervalAdjusts = new float[] { 22.5f, 16.0f, 8.25f };

    //Spawns a specific NPC based on how many have spawned previously
    //E.G: If game spawned 3 NPC's and this will spawn on the 4th, use the NPC type for that time
    Dictionary<int, NPCSpawnpoint.NPCToSpawn> spawnUniqueNPCs = new Dictionary<int, NPCSpawnpoint.NPCToSpawn>();

    int curStage;

    /// <summary>
    /// Gets the player game object
    /// </summary>
    public static GameObject GetPlayerReference() 
        => GameObject.FindGameObjectWithTag("Player");

    public static SurvivalManager survivalInstance;

    void Start()
    {
        SetNPCSpawnTimes();

        survivalInstance = this;

        curTimeInterval = startTimeInterval;
        curStage = 0;
    }

    bool canSpawn = true;

    //Makes an NPC spawn based on how many have spawned previously
    //THIS IS NOT SPAWN TIME INTERVALS
    void SetNPCSpawnTimes()
    {
        //BioOrganics
        spawnUniqueNPCs.Add(3, NPCSpawnpoint.NPCToSpawn.BioOrganic);
        spawnUniqueNPCs.Add(6, NPCSpawnpoint.NPCToSpawn.BioOrganic);

        //Brutes
        spawnUniqueNPCs.Add(11, NPCSpawnpoint.NPCToSpawn.Brute);
    }

    //Pauses survival timer, this also pauses NPC spawning
    [HideInInspector]
    public bool pauseTimer = false;

    void Update()
    {
        //Toggles game console
        if (Input.GetKeyDown(KeyCode.F1))
            GameConsole.ToggleConsole();

        //Stop here if the timer is paused
        if (pauseTimer) return;

        //If player is alive increase timer otherwise stop here
        if (IsPlayerAlive())
            timeSurvived += 1.0f * Time.deltaTime;
        else return;

        //If a time intevral has been met..
        if (Math.Round(timeSurvived, 2) % curTimeInterval == 0 && canSpawn)
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
            }
        }
        else if (Math.Round(timeSurvived, 2) % curTimeInterval == 1)
            canSpawn = true;
    }

    //Spawns a NPC at a random spawnpoint
    void SpawnNPC()
    {
        var point = NPCSpawnpoint.FindRandomSpawnpoint();

        if (spawnUniqueNPCs.Any(k => k.Key == timesSpawnedNPC))
            point.SpawnNPC(spawnUniqueNPCs[timesSpawnedNPC]);
        else
        {
            if (timesSpawnedNPC < 11)
                point.SpawnNPC(NPCSpawnpoint.NPCToSpawn.Brainless);
            else
                point.SpawnNPC(NPCSpawnpoint.NPCToSpawn.Random);
        }

        timesSpawnedNPC++;
    }

    //Returns if the player is alive
    bool IsPlayerAlive()
        => GetPlayerReference().GetComponent<HealthStatistic>().CurHealth > 0.0f;
}
