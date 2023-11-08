using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawnpoint : MonoBehaviour
{
    [SerializeField]
    Object brainless;

    [SerializeField]
    Object bioOrganic;

    [SerializeField]
    Object brute;

    [Tooltip("Radius check for spawning if player is too close")]
    public float radius = 12.0f;

    /// <summary>
    /// Enum of NPC types
    /// </summary>
    public enum NPCToSpawn
    {
        Random, //Spawn a random NPC
        Brainless, //Spawns a brainless zombie
        BioOrganic, //Spawns the goo creature
        Brute //Spawns a bulky creature
    }

    /// <summary>
    /// Spawns a NPC at this point
    /// </summary>
    /// <param name="spawnType">The npc to spawn</param>
    public void SpawnNPC(NPCToSpawn spawnType = NPCToSpawn.Random)
    {
        Object npc = null;

        switch (spawnType)
        {
            case NPCToSpawn.Random: npc = RandomNPC(); break;
            case NPCToSpawn.Brainless: npc = brainless; break;
            case NPCToSpawn.BioOrganic: npc = bioOrganic; break;
            case NPCToSpawn.Brute: npc = brute; break;
        }

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 8.0f, NavMesh.AllAreas))
        {
            Instantiate(npc, hit.position, transform.localRotation);
        }
    }

    //Decides a random NPC
    Object RandomNPC()
    {
        switch(Random.Range(1, 3))
        {
            case 1: return brainless;
            case 2: return bioOrganic;
            case 3: return brute;
        }

        return null;
    }

    /// <summary>
    /// Finds a random spawnpoint present in the level
    /// </summary>
    /// <returns>A npc spawnpoint found or null</returns>
    public static NPCSpawnpoint FindRandomSpawnpoint()
    {
        //Group all spawnpoints that can spawn NPCs into a list
        List<NPCSpawnpoint> spawnpoints = FindObjectsByType<NPCSpawnpoint>(FindObjectsSortMode.None)
            //Picks points that can spawn, ones that return false are rejected
            .Where(s => s.CanSpawn())
            .ToList();

        //Get the max amount of spawns available
        int maxSpawns = spawnpoints.Count;

        //Return a random spawnpoint
        return spawnpoints[Random.Range(0, maxSpawns)];
    }

    /// <summary>
    /// Can an NPC be spawned at this point? E.G, is distant from the player
    /// </summary>
    /// <returns>If spawning can occur</returns>
    bool CanSpawn()
    {
        //Get the player transform position
        var playerPos = SurvivalManager.GetPlayerReference().transform.position;

        if(Physics.Raycast(transform.position, playerPos, out RaycastHit hit))
            //If the player is in LoS of spawnpoint regardless of radius, return false
            if (hit.collider.gameObject.tag == "Player") return false;

        //True if player is further away from radius check
        return Vector3.Distance(transform.position, playerPos) > radius;
    }
}
