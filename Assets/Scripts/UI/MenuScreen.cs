using Abertay.Analytics;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    [SerializeField]
    GameObject startPanel;

    [SerializeField]
    GameObject statPanel;

    public static MenuScreen instance;

    void Awake()
    {
        instance = this;
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void StartPlaying()
    {
        SurvivalManager.instance.pauseTimer = false;
        startPanel.SetActive(false);

        HealthStatistic.allowControls = true;

        //Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetStatisticTexts(string killer, float time, float dist)
    {
        Text killerInfo = GameObject.Find("KillerInfo").gameObject.GetComponent<Text>();
        Text timeDistInfo = GameObject.Find("TimeDistInfo").gameObject.GetComponent<Text>();

        killerInfo.text = $"You were killed by a {killer}";
        
        int seconds = (int)(time % 60);
        int minutes = (int)(time / 60);

        timeDistInfo.text = $"You survived for [{minutes} minutes, {seconds} seconds] while travelling {dist} meters";
    }

    public void ShowStatistics()
    {
        SurvivalManager.instance.pauseTimer = true;
        statPanel.SetActive(true);

        //Shows the cursor but confined in-game screen
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
