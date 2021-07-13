using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenAndClose : MonoBehaviour
{
    public bool TesteHideMouse = false;

    public void EventTeste(bool openornot)
    {
       if (openornot)//If when console open you can unlock the mouse or whatever you want to do
       {
            if (TesteHideMouse){
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            Debug.Log("Console Open");
       }
       else// here is the same, but this is when console close
       {
            if (TesteHideMouse){
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
           Debug.Log("Console Close");
       }
    }
}
