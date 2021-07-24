using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string v_version = "in-dev y21w01";

    public bool v_isplaying { get; private set; }
    public GameObject playerobj;
    private void Awake()
    {
        Game.GameManager = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        print("GameVersion: " + v_version);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Diconnect();
        }
    }

    private void OnDestroy()
    {
        Game.GameManager = null;
    }

    public void StartGame()
    {
        if (v_isplaying) { return; }

        SceneManager.LoadSceneAsync("myworld");
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 2)//start playing, the map is ready to player
        {
            v_isplaying = true;
            Game.GUIManager.StartPlaying();
            Instantiate(playerobj);
        }
    }

    private void DestroyAllObjects()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.tag != "NoDestroy")
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

        Network.Disconnect();
        

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
    public static GameManager GameManager;
    public static Network Network;
    public static GUIManager GUIManager;
    public static MatchMaking MatchMaking;
    public static EntityManager EntityManager;
    public static WorldManager WorldManager;

    #region MenuManagerFunctiopns
    public static void OpenMenu(string name)
    {
        GUIManager?.OpenMenu(name);
    }

    public static void CloseMenu(string name)
    {
        GUIManager?.CloseMenu(name);
    }
    #endregion
}