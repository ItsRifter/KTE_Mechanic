using System;
using System.Linq;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    float timeSurvived = -10.0f;

    const float startTimeInterval = 25.0f;

    float curTimeInterval;

    int timesSpawnedNPC;

    int[] difficultyTweaks = new int[] { 4, 9, 16, 20 };
    float[] intervalAdjusts = new float[] { 22.5f, 16.0f, 8.25f };

    int curStage;

    public static GameObject GetPlayerReference() 
        => GameObject.FindGameObjectWithTag("Player");

    void Start()
    {
        curTimeInterval = startTimeInterval;
        curStage = 0;
    }

    bool canSpawn;

    void Update()
    {
        //If player is alive increase timer otherwise stop here
        if (IsPlayerAlive())
            timeSurvived += 1.0f * Time.deltaTime;
        else return;

        //If a time intevral has been met..
        if (Math.Round(timeSurvived, 2) % curTimeInterval == 0 && canSpawn)
        {
            //Spawn a new NPC
            SpawnNPC();

            //If enough NPCs have spawned, increase the difficulty
            if (difficultyTweaks.Contains(timesSpawnedNPC))
            {
                //Safety check if exceeding interval adjustment array
                if (curStage > intervalAdjusts.Length) return;

                curTimeInterval = intervalAdjusts[curStage];
                curStage++;
            }

            canSpawn = false;
        } 
        else
            canSpawn = true;
    }

    //Spawns a NPC at a random spawnpoint
    void SpawnNPC()
    {
        Debug.Log("Spawning NPC");

        var point = NPCSpawnpoint.FindRandomSpawnpoint();
        point.SpawnNPC(NPCSpawnpoint.NPCToSpawn.Brainless);

        timesSpawnedNPC++;
    }

    //Returns if the player is alive
    bool IsPlayerAlive() 
        => GameObject.FindGameObjectWithTag("Player").GetComponent<HealthStatistic>().CurHealth > 0.0f;
}
