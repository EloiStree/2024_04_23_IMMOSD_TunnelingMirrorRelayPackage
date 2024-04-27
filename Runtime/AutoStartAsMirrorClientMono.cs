using Mirror;
using Mirror.Examples.Basic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStartAsMirrorClientMono : MonoBehaviour
{
    public BasicNetManager m_networkManager;
    public bool m_useStart = true;
    public float m_delay=1;


    void Start()
    {
        if (m_useStart) {
            Invoke("LaunchClient", m_delay);
        }   
    }

    private void LaunchClient()
    {
        if(!m_networkManager.isNetworkActive)
            m_networkManager.StartClient();
    }

}
