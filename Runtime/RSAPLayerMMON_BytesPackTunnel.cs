using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RSAPLayerMMON_BytesPackTunnel : NetworkBehaviour
{

    static RSAPLayerMMON_BytesPackTunnel m_hostInstance;
    public static RSAPLayerMMON_BytesPackTunnel GetHostInstance() { return m_hostInstance; }

    public static void AddListener(Action<string, byte[]> receivedChunk)
    {
        m_onPackageReceived += receivedChunk;
    }

    public static void RemoveListener(Action<string, byte[]> receivedChunk)
    {
        m_onPackageReceived -= receivedChunk;
    }

    public static Action<RSAPLayerMMON_BytesPackTunnel> m_onInstanceSet;

    public static Action<string, byte[]> m_onPackageReceived;
    private static void SetInstanceAndNotifyListener(RSAPLayerMMON_BytesPackTunnel tunnel)
    {
        m_hostInstance = tunnel;
        if(m_onInstanceSet!=null)
            m_onInstanceSet.Invoke(tunnel);

    }
    private static void PushPacktoListeners(string arrayName, byte[] pack)
    {
        if (m_onPackageReceived != null)
        {
            m_onPackageReceived.Invoke(arrayName, pack);
        }
    }



    public string   m_arrayName;
    public byte[]   m_pack;


    public bool m_useRandomPush;

    [SyncVar]
    public bool m_isTheHost;

    public override void OnStartServer()
    {
        m_isTheHost = base.isServer && base.isOwned && base.isClient ;
     
       
    }
    public void Update()
    {
        if (!m_isTheHost)
        {
            DestroyImmediate(this);
        }
        else {

            if (m_hostInstance == null) { SetInstanceAndNotifyListener(this); }
            
        }
    }

   

    private void Start()
    {
      
        InvokeRepeating("PushRandom", 3, 5);
    }
    [ContextMenu("PushRandom")]
    public void PushRandom() {

        if (!m_useRandomPush)
            return;

        m_arrayName = Guid.NewGuid().ToString();
        m_pack = Encoding.UTF8.GetBytes(m_arrayName);
        if (isServer)
            PushBytesServerToClients();
        else PushBytesClientSourceToServer();
    }

    [ContextMenu("PushBytesSoureClientToServer")]
    public void PushBytesClientSourceToServer()
    {
        PushBytesClientSourceToServer(m_arrayName, m_pack);
    }

    public void PushBytesClientSourceToServer(string arrayName,  byte[] pack)
    {

        m_arrayName = arrayName;
        m_pack = pack;

        CmdPushBytesClientSourceToServer(arrayName,  pack);
    }


    [Command]
    private void CmdPushBytesClientSourceToServer(string arrayName,  byte[] pack)
    {

        m_arrayName= arrayName;
        m_pack= pack;

        RpcPushByteToClients(arrayName, pack);
    }

    [ContextMenu("PushBytesServerToClient")]
    public void PushBytesServerToClients()
    {
        PushBytesServerToClients(m_arrayName, m_pack);
    }

    public void PushBytesServerToClients(string arrayName, byte[] pack)
    {

        m_arrayName = arrayName;
        m_pack = pack;

        RpcPushByteToClients(arrayName, pack);
    }
    [ClientRpc]
    private void RpcPushByteToClients(string arrayName, byte[] pack)
    {

        m_arrayName = arrayName;
        m_pack = pack;
        PushPacktoListeners(arrayName, pack);

    }

  
}
