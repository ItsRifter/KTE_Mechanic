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

    /* LIKELY NOT NEEDED
    enum BehavingStatus
    {
        None, //NPC is inactive
        Searching, //Looking for the player
        Hunting, //Lost sight of player (but has found) and is hunting
        Attacking //Sees the player and is attacking
    }

    BehavingStatus curBehaviour;*/

    void Awake()
    {

    }

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
        if ( hp.CurHealth <= 0.0f ) return;

        lastAttackTime = 0f;
        hp.TakeDamage(baseDamage);
    }

    //Sets statistics to this NPC
    void SetStats()
    {
        GetComponent<NPCNav>().navAgent.speed = GetPatrolSpeed();

        var eyeFOV = GetComponent<EyeFOV>();
        eyeFOV.angleFOV = baseFOV;
        eyeFOV.radius = baseRadius;
    }

    //Getters methods
    public float GetPatrolSpeed() => baseSpeed;
    public float GetFOV() => baseFOV;
    public float GetRadius() => baseRadius;
}
