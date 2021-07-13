using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuiItem : MonoBehaviour, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string v_guiTag = "none_none";

    public Component v_guiComponent;

    public bool v_drag = false;
    public bool v_hoverMouse = false;

    public void Start()
    {
        Game.GUIManager.Add(this);
    }

    public void OnDestroy()
    {
        Game.GUIManager.Remove(this);
    }

    public void OnClick()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (v_drag)
        {

        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (v_drag)
        {

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (v_hoverMouse)
        {

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (v_hoverMouse)
        {

        }
    }
}
