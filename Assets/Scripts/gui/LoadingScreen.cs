using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance { get; private set; }

    public GameObject v_loadingScreenObject;
    public Slider v_loadingSlider;
    public TextMeshProUGUI v_loadingText;

    public static bool v_isLoading { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public static void EnableLoadingScreen()
    {
        instance.v_loadingScreenObject.SetActive(true);
        v_isLoading = true;
    }
    public static void DisableLoadingScreen()
    {
        instance.v_loadingScreenObject.SetActive(false);
        v_isLoading = false;
    }

    public static void SetData(string textInfo, float progress)
    {
        instance.v_loadingText.text = textInfo;
        instance.v_loadingSlider.value = progress;
    }
}
