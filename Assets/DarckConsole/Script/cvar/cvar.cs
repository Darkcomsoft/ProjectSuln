using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class cvar
{
    public string cvarCommand { get; private set; }
    public PermCvarFlags permission_flags { get; private set; }
    

    public cvar(string commandString, PermCvarFlags permissionFlags, bool print)
    {

    }

    public virtual void Invoke(params object[] param)
    {

    }
}

public enum PermCvarFlags
{
    All, System, Admin, User
}