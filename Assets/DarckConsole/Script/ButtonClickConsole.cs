using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickConsole : MonoBehaviour
{
    public string commandname = "";

    public void Click()
    {
        ConsoleInGame.Instance.OnClickButao(commandname);
    }
}
