using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSAP_PushIntegerToServer : NetworkBehaviour
{
     static RSAP_PushIntegerToServer m_instance;
    public static RSAP_PushIntegerToServer GetInstanceLocalPlayer() { return m_instance; }

    public int m_lastReceivedFromClient;
    void Start()
    {

        bool isLocalOrServer = this.isServer || this.isLocalPlayer;
        if (isLocalOrServer)
        {
            m_instance = this;
        }
        else { 
            Destroy(this);
        }
    }


    [ContextMenu("Push random integer ")]
    public void PushIntegerRandom() {

        PushInteger(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
    }

    public void PushInteger(int value) {

        CmdPushIntegerToServer(value);
    }

    [Command]
    void CmdPushIntegerToServer(int value)
    {
        m_lastReceivedFromClient = value;

    }
    
}
