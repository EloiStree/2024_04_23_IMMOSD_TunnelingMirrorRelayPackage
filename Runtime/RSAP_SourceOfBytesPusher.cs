using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSAP_SourceOfBytesPusher : NetworkBehaviour
{
     static RSAP_SourceOfBytesPusher m_instance;
    public static RSAP_SourceOfBytesPusher GetHostInstance() { return m_instance; }
    // Allows if your handshake is valide and the public key is allow on the server to push bytes to broadcast to clients.

    void Start()
    {

        bool isLocalOrServer = this.isServer || this.isLocalPlayer;
        if (isLocalOrServer)
        {
            if (isOwned) { 
                m_instance = this;
            }
        }
        else
        {
            Destroy(this);
        }
    }


    public void PushBytesChunkOnServer(string arrayName, byte[] bytesValue)
    {

        CmdPushBytesChunk(arrayName, bytesValue);
    }

    [Command]
     void CmdPushBytesChunk(string arrayName, byte[] bytesValue)
    {
        var i = RSAPLayerMMON_BytesPackTunnel.GetHostInstance();
        if (i != null) {

            i.PushBytesServerToClients(arrayName, bytesValue);
        }
    }




}
