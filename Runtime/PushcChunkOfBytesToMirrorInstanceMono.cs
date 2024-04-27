using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PushcChunkOfBytesToMirrorInstanceMono : MonoBehaviour
{
    public void PushChunkArray(string nameChunkArray, byte[] chunkArray)
    {
        RSAP_SourceOfBytesPusher i = RSAP_SourceOfBytesPusher.GetHostInstance();
        if (i != null)
        {
            i.PushBytesChunkOnServer(nameChunkArray, chunkArray);
        }
    }
    [ContextMenu("Push Random")]
    public void PushChunkArrayRandom()
    {
        string t = Guid.NewGuid().ToString();
        PushChunkArray(t, Encoding.UTF8.GetBytes(t));
    }
    [ContextMenu("Push Random")]
    public void PushChunkArrayGuidArrayRandom()
    {
        string t = Guid.NewGuid().ToString();
        PushChunkArray("Guid", Encoding.UTF8.GetBytes(t));
    }



}