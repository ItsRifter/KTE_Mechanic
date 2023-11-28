using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct VariantType
{
    public float speed;
    public float fov;
    public float radius;
    public Material materialOverride;
}

public static class NPCTypeStatExtensions
{
    /// <summary>
    /// Gets NPC statistics if valid or null
    /// </summary>
    /// <param name="gameObj">The object to get statistics</param>
    /// <returns>The statistics for this NPC game object</returns>
    public static NPCTypeStats GetNPCStatistics(this GameObject gameObj) => gameObj.GetComponent<NPCTypeStats>() ?? null;
}

public class NPCTypeStats : MonoBehaviour, ISingleton
{
    public string npcName = "Dummy";

    [HideInInspector]
    public NavMeshAgent navAgent;

    //The base speed of this type
    public float baseSpeed = 6.0f;
    
    //The FOV of this type
    [Range(0f, 360f)]
    public float baseFOV = 45.0f;

    //The viewing radius of how far the type can see
    public float baseRadius = 5.0f;

    //How much damage to deal
    public float baseDamage = 1.0f;
    
    //The time for a new attack to occur
    public float attackTime = 1.0f;

    //How far does the attack reach
    public float attackRange = 1.5f;

    float lastAttackTime;

    GameObject refPlayer;

    /// <summary>
    /// How the NPC behaves when approaching game objects
    /// </summary>
    //This is only used for door interactions
    public enum BrainBehaviour
    {
        None,           //No unique behaviour
        Zombie_Like,    //Behaves similar to zombies, low brain functions
        Phase_Shifting, //Is capable of phasing past objects
        Aggressive      //A destructive behaviour only causing damage to environment or the living
    }

    public BrainBehaviour behaviourType = BrainBehaviour.None;

    void Start()
    {
        refPlayer = SurvivalManager.GetPlayerReference();
        SetStats();
    }

    void Update()
    {
        if (lastAttackTime < attackTime)
        {
            lastAttackTime += 1.0f * Time.deltaTime;
            lastAttackTime = Mathf.Clamp(lastAttackTime, 0.0f, attackTime);
        }

        if (CanAttackPlayer())
            AttackPlayer();
    }

    bool CanAttackPlayer()
    {
        float distToPlayer = Vector3.Distance(transform.position, refPlayer.transform.position);

        if (distToPlayer <= attackRange && lastAttackTime >= attackTime)
            return true;

        return false;
    }

    void AttackPlayer()
    {
        HealthStatistic hp = refPlayer.GetComponent<HealthStatistic>();

        if (lastAttackTime < attackTime) return;
        if (hp == null || hp.CurHealth <= 0.0f ) return;

        lastAttackTime = 0f;
        hp.TakeDamage(baseDamage, gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
    }

    //Sets statistics to this NPC
    void SetStats()
    {
        GetComponent<NPCNav>().navAgent.speed = baseSpeed;

        var eyeFOV = GetComponent<EyeFOV>();
        eyeFOV.angleFOV = baseFOV;
        eyeFOV.radius = baseRadius;
    }

    //Getters methods
    public float GetPatrolSpeed() => baseSpeed;
    public float GetFOV() => baseFOV;
    public float GetRadius() => baseRadius;
}
