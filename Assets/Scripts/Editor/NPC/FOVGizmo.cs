using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCTypeStats))]
public class FOVGizmo : Editor
{
    void OnSceneGUI()
    {
        EyeFOV fov = target.GetComponent<EyeFOV>();
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angleFOV / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleFOV / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if(fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.refPlayer.transform.position);
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleDegree)
    {
        angleDegree += eulerY;

        return new Vector3(Mathf.Sin(angleDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegree * Mathf.Deg2Rad));
    }
}
