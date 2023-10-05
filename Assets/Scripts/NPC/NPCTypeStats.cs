using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField, Tooltip("Can this NPC have different variants")]
    bool allowVariants = false;

    [SerializeField, InspectableIf("allowVariants")]
    VariantType[] variants;

    public float baseSpeed = 6.0f;
    
    [Range(0f, 360f)]
    public float baseFOV = 45.0f;

    public float baseRadius = 5.0f;

    bool isVariant;

    void Awake()
    {
        float chance = Random.Range(0.1f, 100.0f);
    }

    public float GetPatrolSpeed() => baseSpeed;
    public float GetFOV() => baseFOV;
    public float GetRadius() => baseRadius;
}
