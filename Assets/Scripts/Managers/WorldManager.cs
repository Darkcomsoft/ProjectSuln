using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    private List<World> v_worldList;

    private World v_currentWorld;

    private void Awake()
    {
        Game.WorldManager = this;
    }

    void Start()
    {
        v_worldList = new List<World>();

        print("Spawning worlds!");
        SpawnWorld("w_devworld");
        SpawnWorld("w_greenworld");
        SpawnWorld("w_testes");
    }

    private void OnDestroy()
    {
        Game.WorldManager = null;
    }

    public void SpawnWorld(string worldName)
    {
        SceneLoader.LoadScene(worldName, LoadSceneMode.Additive);
    }

    public void JoinWorld(World world)
    {
        if (v_currentWorld != null)
        {
            v_currentWorld = null;
        }

        v_currentWorld = world;
        v_worldList.Add(world);
    }

    public void AddWorld(World world)
    {
        v_worldList.Add(world);
    }

    public void RemoveWorld(World world)
    {
        v_worldList.Remove(world);
    }

    public static void LoadMainMenu()
    {

    }
}
