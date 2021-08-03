using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private bool v_isquitting;

    public static SceneLoader instance { get; private set; }

    private Queue<SceneQueue> v_sceneQueue;

    private bool v_isloading;
    private SceneLoading v_currentScene;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Application.quitting += Application_quitting;
        v_sceneQueue = new Queue<SceneQueue>();
    }

    private void Application_quitting()
    {
        v_isquitting = true;
    }

    private void Update()
    {
        if (v_isloading)
        {
            if (v_currentScene != null)
            {
                LoadingScreen.SetData("Loading Scene: " + v_currentScene.v_sceneName, v_currentScene.v_asyncOperation.progress);
            }
        }
    }

    public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (instance.v_isquitting) { return; }

        SceneQueue scenen = new SceneQueue(sceneName, loadSceneMode);
        instance.v_sceneQueue.Enqueue(scenen);

        if (instance.v_sceneQueue.Count <= 1)
        {
            instance.LoadSceneQueue();
        }
    }

    private void onCurrentMapComplete(AsyncOperation ansync)
    {
        if (ansync.isDone)
        {
            if (v_sceneQueue.Count <= 0)
            {
                if (v_currentScene.v_asyncOperation == ansync)
                {
                    v_currentScene = null;
                }
                v_isloading = false;
                LoadingScreen.DisableLoadingScreen();
            }
            else
            {
                LoadSceneQueue();
            }
        }
    }

    private void LoadSceneQueue()
    {
        SceneQueue scenen = v_sceneQueue.Dequeue();
        v_currentScene = new SceneLoading();
        v_isloading = true;
        v_currentScene.v_asyncOperation = SceneManager.LoadSceneAsync(scenen.v_sceneName, scenen.v_LoadSceneMode);
        v_currentScene.v_asyncOperation.completed += onCurrentMapComplete;
        LoadingScreen.EnableLoadingScreen();
    }
}

public class SceneLoading
{
    public AsyncOperation v_asyncOperation;
    public string v_sceneName;
}

public struct SceneQueue
{
    public string v_sceneName;
    public LoadSceneMode v_LoadSceneMode;

    public SceneQueue(string v_sceneName, LoadSceneMode v_LoadSceneMode)
    {
        this.v_sceneName = v_sceneName;
        this.v_LoadSceneMode = v_LoadSceneMode;
    }
}