using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Canvas v_guiCanvas;

    public GameObject v_inGameGui;
    public GameObject v_MainMenuGui;

    private void Awake()
    {
        Game.GUIManager = this;    
    }

    void Start()
    {

    }

    private void OnDestroy()
    {
        Game.GUIManager = null;
    }
}