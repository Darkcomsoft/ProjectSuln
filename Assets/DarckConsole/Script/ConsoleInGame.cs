using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.Events;
using System.Threading;

[System.Serializable]
public class OnOpenClose : UnityEvent<bool>
{
}


public class ConsoleInGame : MonoBehaviour
{
    public static ConsoleInGame Instance { get; private set; }
    public string ConsoleVersion = "V0.1";

    public RectTransform InveRoot;
    public RectTransform HelpRoot;
    public GameObject ConsoleWindow;
    public GameObject SlotPrefab;
    public GameObject HelpPrefab;
    public bool IsVisible = false;
    public bool Collapse = true;
    [Header("Input Console")]
    public InputField InputConsole;

    public OnOpenClose OnOpenClose = new OnOpenClose();

    public List<string> List = new List<string>();
    private Dictionary<string, cvar> v_cvarDictionary;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        v_cvarDictionary = new Dictionary<string, cvar>();

        AddInRoolGUI("DarckConsole : " + ConsoleVersion, false, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1), 16);
        Application.logMessageReceived += HandleLog;
        PrintConsole("Debug mode activated.", true, Color.yellow);
        LoadCommands();
    }

    private void LoadCommands()
    {
        PrintConsole("Commands Loading...", true, Color.yellow);

        v_cvarDictionary.Add("help", new HelpCvar("help", PermCvarFlags.All, true));
        v_cvarDictionary.Add("time_print", new PrintTimeCvar("time_print", PermCvarFlags.All, true));
        v_cvarDictionary.Add("teste", new TesteCvar("teste", PermCvarFlags.All, true));
        v_cvarDictionary.Add("buffer", new TesteBuffer("buffer", PermCvarFlags.All, true));

        PrintConsole("Commands Loaded", true, Color.yellow);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            if (IsVisible != true)
            {
                IsVisible = true;
                // Add On Close
                OnOpenClose.Invoke(true);
                ConsoleWindow.SetActive(true);

                InputConsole.ActivateInputField();
            }
            else
            {
                IsVisible = false;
                OnOpenClose.Invoke(false);
                ConsoleWindow.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!string.IsNullOrEmpty(InputConsole.text))
            {
                PrintConsole(InputConsole.text, true, Color.white);
                string[] textarray = InputConsole.text.Split(" "[0]);
                InsertCommand(textarray);
                InputConsole.text = "";//Clear Input
                InputConsole.ActivateInputField();
            }
        }
        Thread.Sleep(1);
    }

    public void OnClickButao(string text_to_input)
    {
        //InputConsole.ActivateInputField();
        InputConsole.text = text_to_input;
        InputConsole.Select();
        foreach (Transform child in HelpRoot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnInputChange()
    {
        if (!string.IsNullOrEmpty(InputConsole.text))
        {
            string[] commands = ProcessHelpCommand(InputConsole.text);

            foreach (Transform child in HelpRoot.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in commands)
            {
                if (item != null)
                {
                    GameObject newAnimal = Instantiate(HelpPrefab) as GameObject;
                    newAnimal.transform.SetParent(HelpRoot.gameObject.transform);
                    newAnimal.transform.localScale = Vector3.one;
                    newAnimal.GetComponent<ButtonClickConsole>().commandname = item;
                    newAnimal.GetComponentInChildren<Text>().text = item;
                }
            }
        }
        else
        {
            foreach (Transform child in HelpRoot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                PrintConsole("<color=red>" + logString + " : </color>" + stackTrace, false, Color.white);
                break;
            case LogType.Assert:
                PrintConsole("<color=red>" + logString + " : </color>" + stackTrace, false, Color.white);
                break;
            case LogType.Exception:
                PrintConsole("<color=red>" + logString + " : </color>" + stackTrace, false, Color.white);
                break;
            case LogType.Warning:
                PrintConsole("<color=yellow>" + logString + " : </color>" + stackTrace, false, Color.white);
                break;
            case LogType.Log:
                PrintConsole("<color=white>" + logString + "</color>", false, Color.white);
                break;
            default:
                PrintConsole("<color=white>" + logString + "</color>", false, Color.white);
                break;
        }
    }

    public void InsertCommand(string[] value)
    {
        //value [0] is the command, [1]>>>> is the values
        //<color=red>Your Text With Color here</color> // To Add Color of Part Of the Text

        string command = value[0].ToLower();

        if (v_cvarDictionary.ContainsKey(command))
        {
            v_cvarDictionary[command].Invoke(value);
        }
        else
        {
            PrintConsole("Don't have this command : " + value[0], true, Color.red);
        }
    }

    public void ClearCanvas()
    {
        if (InveRoot != null)
        {
            foreach (Transform child in InveRoot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    void ClearConsole()
    {
        List.Clear();
        ClearCanvas();
    }

    public void AddInRoolGUI(string text, bool command, Color text_color, int size)
    {
        if (List.Count >= 500)
        {
            List.Clear();
        }

        if (List.Contains(text) == false || command == true || Collapse == false)
        {
            if (InveRoot)
            {
                List.Add(text);

                GameObject newAnimal = Instantiate(SlotPrefab) as GameObject;

                newAnimal.GetComponent<Text>().text = text;
                newAnimal.GetComponent<Text>().color = text_color;
                newAnimal.GetComponent<Text>().fontSize = size;
                newAnimal.transform.SetParent(InveRoot.gameObject.transform);
                newAnimal.transform.localScale = Vector3.one;
                newAnimal.GetComponent<RectTransform>().sizeDelta = new Vector2(newAnimal.GetComponent<RectTransform>().sizeDelta.x, 14 + size);
            }
        }
    }

    public static void PrintConsole(string text, bool command, Color text_color)
    {
        if (Instance.List.Count >= 500)
        {
            Instance.List.Clear();
        }

        if (Instance.List.Contains(text) == false || command == true || Instance.Collapse == false)
        {
            if (Instance.InveRoot)
            {
                Instance.List.Add(text);
                GameObject newAnimal = Instantiate(Instance.SlotPrefab) as GameObject;
                newAnimal.GetComponent<Text>().text = text;
                newAnimal.GetComponent<Text>().color = text_color;
                newAnimal.transform.SetParent(Instance.InveRoot.gameObject.transform);
                newAnimal.transform.localScale = Vector3.one;
            }
        }
    }

    public string[] ProcessHelpCommand(string Command)
    {
        string[] commands = GetCommands(Command);

        for (int i =0; i < commands.Length; i++)
        {
            if (string.Equals(commands[i], "connect", StringComparison.OrdinalIgnoreCase))
            {
                commands[i] += " 127.0.0.1 2500 ";
            }
            else if (string.Equals(commands[i], "fpsmax", StringComparison.OrdinalIgnoreCase))
            {
                commands[i] += " 60";
            }
            else if (string.Equals(commands[i], "console.collapse", StringComparison.OrdinalIgnoreCase))
            {
                commands[i] += " true";
            }
        }

        return commands;
    }

    public string[] GetCommands(string command)
    {
        List<string> commandsfound = new List<string>();

        command = command.ToLower();

        foreach (var item in v_cvarDictionary)
        {
            char[] characters = command.ToCharArray();
            string letterdound = "";

            for (int i =0; i < characters.Length; i++)
            {
                letterdound += characters[i].ToString();
            }

            if (item.Key.Contains(letterdound))
            {
                if (!commandsfound.Contains(item.Key))
                {
                    commandsfound.Add(item.Key);
                }
            }

            if (string.Equals(letterdound.ToString(), item.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (!commandsfound.Contains(item.Key))
                {
                    commandsfound.Add(item.Key);
                }
            }
        }
        return commandsfound.ToArray();
    }
}