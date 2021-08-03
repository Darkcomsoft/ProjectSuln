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

    public static void SpawnObject(GameObject gameObject, Vector2 position, string worldName)
    {
        GameObject obj = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(worldName));
    }

    public static void SpawnObject(GameObject gameObject, Quaternion rotation, string worldName)
    {
        GameObject obj = GameObject.Instantiate(gameObject, Vector3.zero, rotation);
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(worldName));
    }

    public static void SpawnObject(GameObject gameObject, string worldName)
    {
        GameObject obj = GameObject.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(worldName));
    }

    public static void SpawnObject(GameObject gameObject, Vector2 position, Quaternion rotation, string worldName)
    {
        GameObject obj = GameObject.Instantiate(gameObject, position, rotation);
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(worldName));
    }

    public static void DestroyObject(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    public static void DestroyObject(GameObject gameObject, float time)
    {
        GameObject.Destroy(gameObject, time);
    }
}
