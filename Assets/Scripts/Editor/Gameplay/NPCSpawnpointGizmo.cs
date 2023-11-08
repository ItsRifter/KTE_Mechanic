using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCSpawnpoint))]
public class NPCSpawnpointGizmo : Editor
{
    void OnSceneGUI()
    {
        NPCSpawnpoint point = target.GetComponent<NPCSpawnpoint>();

        Handles.color = Color.yellow;
        Handles.DrawWireArc(point.transform.position, Vector3.up, Vector3.forward, 360, point.radius);
    }
}

