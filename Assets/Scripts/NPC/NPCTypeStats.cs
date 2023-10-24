using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VariantType
{
    public float speed;
    public float fov;
    public float radius;
    public Material materialOverride;
}

public class NPCTypeStats : MonoBehaviour
{
    //Allows variants of this npc type
    //MIGHT NOT BE NEEDED
    //[SerializeField, Tooltip("Can this NPC have different variants")]
    //bool allowVariants = false;

    //The variants of this npc, used for spawning
    [SerializeField]
    VariantType[] variants;

    //The base speed of this type
    public float baseSpeed = 6.0f;
    
    //The FOV of this type
    [Range(0f, 360f)]
    public float baseFOV = 45.0f;

    //The viewing radius of how far the type can see
    public float baseRadius = 5.0f;

    public float baseDamage = 1.0f;

    //bool isVariant;

    enum BehavingStatus
    {
        None, //NPC is inactive
        Searching, //Looking for the player
        Hunting, //Lost sight of player (but has found) and is hunting
        Attacking //Sees the player and is attacking
    }

    BehavingStatus curBehaviour;
    
    void Awake()
    {
        float chance = Random.Range(0.1f, 100.0f);
    }

    void Start()
    {
        SetStats();
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
