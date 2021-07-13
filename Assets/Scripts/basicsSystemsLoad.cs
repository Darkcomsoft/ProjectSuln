using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class basicsSystemsLoad : MonoBehaviour
{
    public GameObject v_steamPrefab;
    private bool v_readytoGo;
    private bool v_done;

    private AsyncOperation v_asyncOperation;

    public Slider v_progressSlider;

    void Start()
    {
        Instantiate(v_steamPrefab);
    }

    void Update()
    {
        if (v_done == false)
        {
            if (SteamManager.v_steamRuning)
            {
                v_readytoGo = true;
                v_done = true;
            }
        }

        if (v_readytoGo)
        {
            v_readytoGo = false;
            v_asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        }

        if (v_asyncOperation != null)
        {
            if (!v_asyncOperation.isDone)
            {
                v_progressSlider.value = v_asyncOperation.progress;
            }
        }
    }
}
