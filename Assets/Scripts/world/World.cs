using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    public string v_worldName = "w_somename";

    private Scene v_currentScene;

    private bool v_playerIsHere = false;
    private List<Entity> v_entityList;

    void Start()
    {
        v_entityList = new List<Entity>();

        v_currentScene = SceneManager.GetSceneByName(v_worldName);
        
        print("CurrentScene: " + v_currentScene.name);
    }

    public void EntityJoin(Entity entity)
    {
        /*if (entity == EntitPlayer.this)//por isso assim que tiver o player spawnando!
        {
            v_playerIsHere = true;
            SceneManager.SetActiveScene(v_currentScene);
        }*/

        if (!v_entityList.Contains(entity))
        {
            v_entityList.Add(entity);
        }
        else
        {
            Debug.LogError("Try to add a entity already exting in list, to the list! somthing is not right!");
        }
    }

    public void EntityLeave(Entity entity)
    {
        /*if (entity == EntitPlayer.this)//por isso assim que tiver o player spawnando!
        {
            v_playerIsHere = false;
        }*/

        if (v_entityList.Contains(entity))
        {
            v_entityList.Remove(entity);
        }
    }
}
