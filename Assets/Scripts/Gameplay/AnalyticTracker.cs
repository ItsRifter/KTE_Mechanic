using Abertay.Analytics;
using System;
using System.Collections.Generic;
using UnityEngine;

//Tracks, collects and sends analytics either locally or to unity services
public class AnalyticTracker : MonoBehaviour
{
    public static AnalyticTracker instance;

    Dictionary<string, object> data;

    float distTravel = 0;
    Vector3 lastPos;

    private void Awake()
    {
        data = new Dictionary<string, object>
        {
            { "killer", "" },
            { "timeSurvived", 0.0f },
            { "distanceTravelled", 0.0f }
        };

        instance = this;

        AnalyticsManager.Initialise("development");
    }

    public void SendAnalytics()
    {
        AnalyticsManager.SendCustomEvent("SurviveAttempt", data);
    }

    public void StoreDataInAnalytics(string killer = "Nothing")
    {
        data["killer"] = killer;

        float time = (float)Math.Round(GetSurvivedTime(), 2);
        data["timeSurvived"] = time;

        data["distanceTravelled"] = distTravel;

        MenuScreen.instance.SetStatisticTexts(killer, time, distTravel);

        SendAnalytics();
    }

    public void UpdateDistanceTravelled(Vector3 pos)
    {
        distTravel += (float)Math.Round(Vector3.Distance(pos, lastPos));
        lastPos = pos;
    }

    public float GetSurvivedTime() => SurvivalManager.instance.timeSurvived;
}
