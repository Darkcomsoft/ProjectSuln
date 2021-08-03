using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<MenuComponent> v_listOfMenus = new List<MenuComponent>();
    private Dictionary<string, MenuComponent> v_menuDictionary;
    private Dictionary<string, GuiItem> v_guiItemsDictionary;

    private void Awake()
    {
        Game.MenuManager = this;
    }

    private void Start()
    {
        v_guiItemsDictionary = new Dictionary<string, GuiItem>();
        v_menuDictionary = new Dictionary<string, MenuComponent>();

        for (int i = 0; i < v_listOfMenus.Count; i++)
        {
            v_menuDictionary.Add(v_listOfMenus[i].name, v_listOfMenus[i]);
        }

        v_listOfMenus.Clear();
    }

    private void OnDestroy()
    {
        v_guiItemsDictionary.Clear();
        v_guiItemsDictionary = null;

        Game.MenuManager = null;
    }

    public void Add(GuiItem gui)
    {
        if (!v_guiItemsDictionary.ContainsKey(gui.v_guiTag))
        {
            v_guiItemsDictionary.Add(gui.v_guiTag, gui);
        }
    }

    public void Remove(GuiItem gui)
    {
        if (v_guiItemsDictionary.ContainsKey(gui.v_guiTag))
        {
            v_guiItemsDictionary.Remove(gui.v_guiTag);
        }
    }

    #region MenuManager
    public void OpenMenu(string name)
    {
        if (v_menuDictionary.ContainsKey(name))
        {
            CloseAll();
            v_menuDictionary[name].gameObject.SetActive(true);
        }
    }

    public void CloseMenu(string name)
    {
        if (v_menuDictionary.ContainsKey(name))
        {
            CloseAll();
            v_menuDictionary[name].gameObject.SetActive(false);
        }
    }

    public void CloseAll()
    {
        foreach (var item in v_menuDictionary)
        {
            item.Value.gameObject.SetActive(false);
        }
    }
    #endregion
}
