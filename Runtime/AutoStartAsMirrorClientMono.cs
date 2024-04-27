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
    public float m_delay = 1;
    public float m_delayBetweenCheck = 5;

    public bool m_checkAutoConnection;

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
        if (m_checkAutoConnection) {

            InvokeRepeating("CheckToReconnect", m_delayBetweenCheck, m_delayBetweenCheck);
        }
    }
    public void CheckToReconnect() {

        if (m_networkManager.isNetworkActive)
            return;

        m_networkManager.StartClient();
    }

}
