using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public const string v_version = "in-dev y21w01";

    public bool v_isplaying { get; private set; }
    public GameObject playerobj;
    private void Awake()
    {
        Game.AppManager = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 0;
        print("GameVersion: " + v_version);

        AssetsManager.LoadAssets();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Game.v_mouseLocked)
            {
                Game.UnLockCursor();
            }
            else
            {
                Game.LockCursor();
            }
        }

        Game.UpdateLockCuror();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Diconnect();
        }
    }

    private void LateUpdate()
    {
        Game.UpdateLockCuror();
    }

    private void OnDestroy()
    {
        Game.UnLockCursor();
        Diconnect();
        Game.AppManager = null;
    }

    public void StartGame()
    {
        if (v_isplaying) { return; }

        SceneLoader.LoadScene("myworld");
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 2)//start playing, the map is ready to player
        {
            v_isplaying = true;
            World.SpawnObject(playerobj, "w_devworld");
        }
    }

    private void DestroyAllObjects()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.layer != 3)
            {
                GameObject.Destroy(go.gameObject);
            }
        }
    }

    #region StartGameWorld
    public void StartGameWorld()
    {

    }

    public void Diconnect()
    {
        if (!v_isplaying) { return; }

        v_isplaying = false;

        Network.Disconnect();
        SceneLoader.LoadScene("MainMenu");
        DestroyAllObjects();
    }
    #endregion
}

public static class Uyilitis
{
    /// <summary>
    /// Generate a unique id. Length is for how long you want to be the id, 1 is normal(short)
    /// </summary>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static int UniqueID(int Length)
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int currentEpochTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        int z1 = UnityEngine.Random.Range(0, 1000000);
        int z2 = UnityEngine.Random.Range(0, 1000);
        return (currentEpochTime / z1 + z2 * Length);
    }
}

/// <summary>
/// Some static functions, and variable, to help connect the game systems
/// </summary>
public static class Game
{
    public static AppManager AppManager;
    public static Network Network;
    public static GUIManager GUIManager;
    public static MenuManager MenuManager;
    public static MatchMaking MatchMaking;
    public static EntityManager EntityManager;
    public static WorldManager WorldManager;

    #region MenuManagerFunctiopns
    public static void OpenMenu(string name)
    {
        MenuManager?.OpenMenu(name);
    }

    public static void CloseMenu(string name)
    {
        MenuManager?.CloseMenu(name);
    }
    #endregion

    #region MouseStuff
    public static bool v_mouseLocked { get; private set; }

    public static void LockCursor()
    {
        v_mouseLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnLockCursor()
    {
        v_mouseLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void UpdateLockCuror()
    {
        if (v_mouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    #endregion
}