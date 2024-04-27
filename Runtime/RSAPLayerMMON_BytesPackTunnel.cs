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

    public string   m_arrayName;
    public int      m_lastStartOffset;
    public int      m_lenghtOffset;
    public byte[]   m_pack;


    public bool m_useRandomPush;

  
    [SyncVar]
    public bool m_isTheHost;


    private void Awake()
    {
        m_isTheHost = this.isLocalPlayer && base.isServer;
        if (m_isTheHost)
        {
            m_hostInstance = this;
        }
        else {
            Destroy(this);
        }
        InvokeRepeating("PushRandom", 3, 5);
    }
    [ContextMenu("PushRandom")]
    public void PushRandom() {

        if (!m_useRandomPush)
            return;

        m_arrayName = Guid.NewGuid().ToString();
        m_lastStartOffset = UnityEngine.Random.Range(0, 9999);
        m_lenghtOffset = UnityEngine.Random.Range(0, 9999);
        m_pack = Encoding.UTF8.GetBytes(m_arrayName);
        if (isServer)
            PushBytesServerToClient();
        else PushBytesSoureClientToServer();
    }

    [ContextMenu("PushBytesSoureClientToServer")]
    public void PushBytesSoureClientToServer()
    {
        PushBytesSoureClientToServer(m_arrayName, m_lastStartOffset, m_lenghtOffset, m_pack);
    }

    public void PushBytesSoureClientToServer(string arrayName, int startOffset, int lenghtOffset, byte[] pack)
    {

        m_arrayName = arrayName;
        m_lastStartOffset = startOffset;
        m_lenghtOffset = lenghtOffset;
        m_pack = pack;

        CmdPushBytesToServer(arrayName, startOffset, lenghtOffset, pack);
    }


    [Command]
    private void CmdPushBytesToServer(string arrayName, int startOffset, int lenghtOffset, byte[] pack)
    {

        m_arrayName= arrayName;
        m_lastStartOffset= startOffset;
        m_lenghtOffset= lenghtOffset;
        m_pack= pack;

        RpcPushByteToClients(arrayName, startOffset, lenghtOffset, pack);
    }

    [ContextMenu("PushBytesServerToClient")]
    public void PushBytesServerToClient()
    {
        PushBytesServerToClient(m_arrayName, m_lastStartOffset, m_lenghtOffset, m_pack);
    }

        public void PushBytesServerToClient(string arrayName, int startOffset, int lenghtOffset, byte[] pack)
    {

        m_arrayName = arrayName;
        m_lastStartOffset = startOffset;
        m_lenghtOffset = lenghtOffset;
        m_pack = pack;

        RpcPushByteToClients(arrayName, startOffset, lenghtOffset, pack);
    }
    [ClientRpc]
    private void RpcPushByteToClients(string arrayName, int startOffset, int lenghtOffset, byte[] pack)
    {

        m_arrayName = arrayName;
        m_lastStartOffset = startOffset;
        m_lenghtOffset = lenghtOffset;
        m_pack = pack;

    }
}
