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
            Vector3 point = hit.position + Vector3.up * 1.0f;
            Instantiate(npc, point, transform.localRotation);
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
        //Group all spawnpoints into a list
        List<NPCSpawnpoint> spawnpoints = FindObjectsByType<NPCSpawnpoint>(FindObjectsSortMode.None).ToList();

        //Get the max amount of spawns available
        int maxSpawns = spawnpoints.Count;

        //Return a random spawnpoint
        return spawnpoints[Random.Range(0, maxSpawns)];
    }

    /// <summary>
    /// Can an NPC be spawned at this point? E.G, is not in LoS or is distant from player
    /// </summary>
    /// <returns>If spawning can occur</returns>
    bool CanSpawn()
    {
        return true;
    }
}
