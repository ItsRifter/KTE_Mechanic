using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static NPCSpawnpoint;

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

    void Start()
    {
        consoleInstance = this;
    }

    /// <summary>
    /// Toggles console visibility
    /// </summary>
    public static void ToggleConsole()
    {
        consoleInstance.showConsole = !consoleInstance.showConsole;

        Cursor.visible = consoleInstance.showConsole;

        //Set mouse lockmode based on console visibility
        Cursor.lockState = consoleInstance.showConsole ? CursorLockMode.Confined : CursorLockMode.Locked;
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
            //Get health component and set health to value
            SurvivalManager.GetPlayerReference().GetComponent<HealthStatistic>().SetHealth(setHP);
        });

        //DOESN'T WORK, this parameter command don't function as intended
        /*var SPAWN_NPC = new ConsoleCommand<string>("npc_spawn", "Spawns an NPC by name input at raycast", (string name) =>
        {
            name = name.ToLower();

            if (name == "brainless" || name == "sentient slime" || name == "brute")
            {
                var camera = Camera.main.transform;

                NPCToSpawn spawnType = NPCToSpawn.Random;

                switch (name)
                {
                    case "brainless": spawnType = NPCToSpawn.Brainless; break;
                    case "sentient slime": spawnType = NPCToSpawn.BioOrganic; break;
                    case "brute": spawnType = NPCToSpawn.Brute; break;
                }

                if(Physics.Raycast(camera.position, camera.forward, out RaycastHit info, 32f ))
                    NPCSpawnpoint.spawnInstance.SpawnNPC(spawnType, info.transform);
            }
        });*/

        var NPC_SPAWN_BRAINLESS = new ConsoleCommand("npc_spawn_zombie", "Spawns the brainless NPC", () =>
        {
            SpawnNPCType(0);
        });

        var NPC_SPAWN_SLIME = new ConsoleCommand("npc_spawn_zombie", "Spawns the brainless NPC", () =>
        {
            SpawnNPCType(1);
        });

        var NPC_SPAWN_BRUTE = new ConsoleCommand("npc_spawn_zombie", "Spawns the brainless NPC", () =>
        {
            SpawnNPCType(2);
        });

        var PLAYER_GODMODE = new ConsoleCommand("godmode", "Makes the player immune to all forms of damage", () =>
        {
            //Get health component and toggle god mode
            var health = SurvivalManager.GetPlayerReference().GetComponent<HealthStatistic>();
            health.onGodMode = !health.onGodMode;
        });

        commandList = new List<object>()
        {
            STOP_SPAWNING,
            SET_HEALTH,
            NPC_SPAWN_BRAINLESS,
            NPC_SPAWN_SLIME,
            NPC_SPAWN_BRUTE,
            PLAYER_GODMODE
        };
    }

    //Spawns an NPC at raycast
    //Types are: 0 = Brainless, 1 = Slime, 2 = Brute
    void SpawnNPCType(int type = 0)
    {
        var camera = Camera.main;
        var playerTransform = SurvivalManager.GetPlayerReference().transform;

        Vector3 forward = camera.transform.position + camera.transform.forward;

        if (Physics.Raycast(camera.transform.position, forward, out RaycastHit hit, 999))
        {
            switch (type)
            {
                case 0: spawnInstance.SpawnNPC(NPCToSpawn.Brainless, hit.transform); break;
                case 1: spawnInstance.SpawnNPC(NPCToSpawn.BioOrganic, hit.transform); break;
                case 2: spawnInstance.SpawnNPC(NPCToSpawn.Brute, hit.transform); break;
            }
        }
    }

    //Prevents inputs from happening if true
    //this makes a command run once instead of multiple in a frame
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
    }

    IEnumerator HandleInputPause()
    {
        haltInputs = true;
        yield return new WaitForSeconds(1.0f);
        haltInputs = false;
    }

    //Handle keyboard actions such as arrow keys, return etc.
    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            OnReturn();

        //NOT NEEDED FOR THIS MODULE,
        //It was a nice to have part but is out of the scope
        /*if(pastCommands.Count != 0)
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

            //If entry index is not -1 and should update, get the command entry
            if (entryIndex != -1 && shouldUpdate)
            {
                input = GetCommandEntry(entryIndex);
            }
        }*/
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            //UpdatePastCommands();

            bool success = HandleInput();
            input = "";

            if (success)
                Debug.Log("CHEATS: Command ran successfully");
            else
                Debug.Log("CHEATS: Command failed");

            StartCoroutine(HandleInputPause());
        }
    }

    //Updates past command array, this either adds to or removes old indexes
    /*void UpdatePastCommands()
    {
        //Past command entries has reached the max allowed, delete the oldest element
        if (pastCommands.Count == maxCommandEntries)
            pastCommands.RemoveAt(0);

        //Add the input to the entries
        pastCommands.Add(input);
    }*/

    string GetCommandEntry(int entry) => pastCommands[entry];

    /// <summary>
    /// Handles the input from the command line
    /// </summary>
    /// <returns>If the command ran successfully or failed</returns>
    bool HandleInput()
    {
        //No input, stop here
        if (string.IsNullOrEmpty(input)) return false;

        string[] parameters = input.Split(' ');

        ConsoleCommandBase command = null;

        for (int i = 0; i < commandList.Count; i++)
        {
            if (commandList[i] is ConsoleCommand cmd && cmd.commandId == input.ToLower())
            {
                command = cmd;
                break;
            }
        }

        //Failed to find the command, stop here
        if (command == null) return false;

        if (command is ConsoleCommand execCmd)
            execCmd.Invoke();
        else
        {
            if (command is ConsoleCommand<float> execFloat)
                execFloat.Invoke( float.Parse(parameters[1]) );

            if(command is ConsoleCommand<string> execString)
                execString.Invoke( parameters[1] );
        }

        return true;
    }
}
