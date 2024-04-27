using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenToBytesOfMirrorInstanceMono : MonoBehaviour
{
    public string m_arrayName;
    public byte[] m_arrayChunk;

    public delegate void ReceivedChunkDelegate(string arrayName, byte[] arrayChunkBytes);
    public ReceivedChunkDelegate m_onChunkDelegateReceived;
    public UnityEvent<string, byte[]> m_onChunkEventReceived;

    void Awake()
    {

        RSAPLayerMMON_BytesPackTunnel.AddListener(ReceivedChunk);


    }

    private void OnDestroy()
    {

        RSAPLayerMMON_BytesPackTunnel.RemoveListener(ReceivedChunk);
    }

    public void ReceivedChunk(string arrayName, byte []  arrayChunkBytes) {

        m_arrayName = arrayName;
        m_arrayChunk = arrayChunkBytes;
        if (m_onChunkDelegateReceived != null)
            m_onChunkDelegateReceived.Invoke(arrayName, arrayChunkBytes);
        m_onChunkEventReceived.Invoke(arrayName, arrayChunkBytes);
    }

}
