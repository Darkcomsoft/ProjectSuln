using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCvar : cvar
{
    public HelpCvar(string commandString, PermCvarFlags permissionFlags, bool print) : base(commandString, permissionFlags, print) { }

    public override void Invoke(params object[] param)
    {
        ConsoleInGame.PrintConsole("Comming Soon!", true, Color.yellow);
        base.Invoke(param);
    }
}

public class PrintTimeCvar : cvar
{
    public PrintTimeCvar(string commandString, PermCvarFlags permissionFlags, bool print) : base(commandString, permissionFlags, print) { }

    public override void Invoke(params object[] param)
    {
        ConsoleInGame.PrintConsole("Current Time: " + System.DateTime.Now.ToString(), true, Color.yellow);
        base.Invoke(param);
    }
}

public class TesteCvar : cvar
{
    public TesteCvar(string commandString, PermCvarFlags permissionFlags, bool print) : base(commandString, permissionFlags, print) { }

    public override void Invoke(params object[] param)
    {
        ConsoleInGame.PrintConsole("TESTES TESTES TESTE!", true, Color.yellow);
        base.Invoke(param);
    }
}

public class TesteBuffer : cvar
{
    public TesteBuffer(string commandString, PermCvarFlags permissionFlags, bool print) : base(commandString, permissionFlags, print) { }

    public override void Invoke(params object[] param)
    {
        NetMsgPacket msg = new NetMsgPacket();
        NetMsgPacket msg2 = new NetMsgPacket();

        msg.Write(0);

        msg.Write(1);

        msg.Write(5);

        msg.Write(8);

        msg.Write(9);

        msg.Write(true);

        msg.Write(false);

        msg.Write("testes");

        msg.Write("testes2");

        byte[] d = msg.Serialize();
        msg2.Deserialize(d);

        Debug.Log("Read: " + msg2.ReadInt());

        Debug.Log("Read: " + msg2.ReadInt());

        Debug.Log("Read: " + msg2.ReadInt());

        Debug.Log("Read: " + msg2.ReadInt());

        Debug.Log("Read: " + msg2.ReadInt());

        Debug.Log("Read: " + msg2.ReadBool());

        Debug.Log("Read: " + msg2.ReadBool());

        Debug.Log("Read: " + msg2.ReadString());

        Debug.Log("Read: " + msg2.ReadString());
        base.Invoke(param);
    }
}