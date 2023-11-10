using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//CODE FROM "Game Dev Guide"
//https://www.youtube.com/watch?v=VzOEM-4A2OM

public class GameConsole : MonoBehaviour
{
    [HideInInspector]
    public bool showConsole;

    string input;

    List<string> pastCommands = new List<string>();

    public static GameConsole consoleInstance;

    public List<object> commandList;

    int maxCommandEntries = 5;

    int entryIndex = -1;

    void Start()
    {
        consoleInstance = this;
    }

    public static void ToggleConsole()
    {
        consoleInstance.showConsole = !consoleInstance.showConsole;

        Cursor.visible = consoleInstance.showConsole;
        Cursor.lockState = consoleInstance.showConsole ? CursorLockMode.Confined : CursorLockMode.Locked;

        consoleInstance.entryIndex = -1;
    }

    void Awake()
    {
        //Stop NPC Spawns
        var STOP_SPAWNING = new ConsoleCommand("toggle_spawns", "Toggles NPCs from spawning, if disabled the survived timer will stop", () =>
        {
            SurvivalManager.instance.pauseTimer = !SurvivalManager.instance.pauseTimer;
        });

        var SET_HEALTH = new ConsoleCommand<float>("player_sethealth", "Sets player health value between 0 and 100 (0 means death)", (float setHP) =>
        {
            SurvivalManager.GetPlayerReference().GetComponent<HealthStatistic>().SetHealth(setHP);
        });

        var SPAWN_NPC = new ConsoleCommand<string>("npc_spawn", "Spawns an NPC by name input at raycast", (string name) =>
        {

        });

        commandList = new List<object>()
        {
            STOP_SPAWNING,
            SET_HEALTH,
            SPAWN_NPC
        };
    }

    bool haltInputs = false;

    void OnGUI()
    {
        if (!showConsole) return;

        float y = 0.0f;

        GUI.Box(new Rect(0, y, Screen.width, 30.0f), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        if(!haltInputs)
            HandleActions();

        haltInputs = false;
    }

    //Handle keyboard actions such as arrow keys, return etc.
    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            OnReturn();

        if(pastCommands.Count != 0)
        {
            //Stops console from updating input field if it shouldn't update
            bool shouldUpdate = false;

            if(Input.GetKeyDown(KeyCode.UpArrow) && !shouldUpdate)
            {
                entryIndex++;
                if (entryIndex > pastCommands.Count-1)
                    entryIndex = 0;

                haltInputs = true;
                shouldUpdate = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && !shouldUpdate)
            {
                entryIndex--;
                if (entryIndex < 0)
                    entryIndex = pastCommands.Count-1;
                
                haltInputs = true;
                shouldUpdate = true;
            }

            Debug.Log(shouldUpdate);

            //If entry index is not -1 and should update, get the command entry
            if (entryIndex != -1 && shouldUpdate)
            {
                input = GetCommandEntry(entryIndex);
            }
        }
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            haltInputs = true;
            HandleInput();
            UpdatePastCommands();
            input = "";
        }
    }

    //Updates past command array, this either adds to or removes old indexes
    void UpdatePastCommands()
    {
        //Past command entries has reached the max allowed, delete the oldest element
        if (pastCommands.Count == maxCommandEntries)
            pastCommands.RemoveAt(0);

        //Add the input to the entries
        pastCommands.Add(input);
    }

    string GetCommandEntry(int entry) => pastCommands[entry];

    void HandleInput()
    {
        //No input, stop here
        if (string.IsNullOrEmpty(input)) return;

        string[] parameters = input.Split(' ');

        ConsoleCommandBase command = null;

        for (int i = 0; i < commandList.Count; i++)
        {
            if (commandList[i] is ConsoleCommand cmd)
            {
                command = cmd;
                break;
            }
        }

        //Failed to find the command, stop here
        if (command == null) return;

        if (command is ConsoleCommand execCmd)
            execCmd.Invoke();
        else
        {
            if (command is ConsoleCommand<float> execFloat)
                execFloat.Invoke( float.Parse(parameters[1]) );

            if(command is ConsoleCommand<string> execString)
                execString.Invoke( parameters[1] );
        }
    }
}
