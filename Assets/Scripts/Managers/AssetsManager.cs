using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetsManager
{
    public static Dictionary<string, GameObject> v_prefabDictionary;

    public static void LoadAssets()
    {
        LoadingScreen.EnableLoadingScreen();
        v_prefabDictionary = new Dictionary<string, GameObject>();

        LoadingScreen.SetData("Loading Prefabs!", 0);



        LoadingScreen.SetData("Object's pool, Completed!", 100);
        SetupObjectPool();
        LoadingScreen.DisableLoadingScreen();
    }

    private static void SetupObjectPool()
    {
        LoadingScreen.SetData("Setting up Object's pool!", 0);


        LoadingScreen.SetData("Object's pool, Completed!", 100);
    }
}