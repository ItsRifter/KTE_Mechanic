using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Struct of a model group
[System.Serializable]
public struct ModelGroupStruct
{
    [Tooltip("The name of this model group, used only for organisation")]
    public string groupName;
    public GameObject[] objects;
    public bool startActive;
}

//Allows switching models similar to Source modelgroups at runtime
public class ModelGroups : MonoBehaviour
{
    [SerializeField]
    ModelGroupStruct[] groups;

    int curGroup = -1;

    // Start is called before the first frame update
    void Start()
    {
        var listCheck = groups.Where(f => f.startActive).ToList();

        //If no groups have startActive set
        if (!listCheck.Any())
        {
            Debug.LogWarning($"{gameObject} has no model groups set on activation, Using first element in groups");

            //Default to the first model group
            SetModelGroup(0);
        }
        //If more than one group has startActive set
        else if (listCheck.Count() > 1)
        {
            Debug.LogWarning($"{gameObject} has multiple modelgroups are set to start on activation, using first group stated for activation");

            //Find the first model group that has startActive set
            SetModelGroup(Array.FindIndex(groups, f => f.startActive));
        }

        SetModelGroup(Array.FindIndex(groups, f => f.startActive));
    }

    /// <summary>
    /// Sets the model group
    /// </summary>
    /// <param name="group">The group to set</param>
    public void SetModelGroup(int group)
    {
        //If the parameter exceeds the model group array length, stop here
        if (group - 1 > groups.Length)
        {
            Debug.LogError($"{gameObject} contains less elements from parameter, ignoring");
            return;
        }

        //If parameter is less than 0, just set to 0 with a warning
        if (group < 0)
        {
            group = 0;
            Debug.Log("Model group parameter exceeds behind 0, setting to 0");
        }

        //Set previous model group visabilty to false and set the new group visability to true

        //If a group is currently set after initialising or not
        if (curGroup != -1)
            groups[curGroup].objects.ToList().ForEach(o => o.SetActive(false));
        else
            foreach (var groupItem in groups)
                groupItem.objects.ToList().ForEach(o => o.SetActive(false));

        groups[group].objects.ToList().ForEach(o => o.SetActive(true));

        curGroup = group;
    }
}
