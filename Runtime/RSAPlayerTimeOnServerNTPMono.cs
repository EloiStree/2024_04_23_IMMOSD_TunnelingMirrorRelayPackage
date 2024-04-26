using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class RSAPlayerTimeOnServerNTPMono : NetworkBehaviour
{

    public static string m_serverToSyncOn = "ntp1.teambelgium.net";

    [SyncVar]
    public double m_timeBetweenPcServerNtp;
    public ComputeServerVsPcDateTime m_serverPcDifference;


    [System.Serializable]
    public class ComputeServerVsPcDateTime { 
        public string m_currentTimeOnPc;
        public string m_currentTimeOnNtp;
        public long m_currentTimeOnPcTick;
        public long m_currentTimeOnNtpTick;
        public long m_differencePcNtpTick;
        public double m_differencePcNtpMilliseconds;
        public DateTime m_currentTimeOnPcDate;
        public DateTime m_currentTimeOnNtpDate;
        public bool m_serverContacted;

        public void Refresh()
        {
            try
            {
                m_serverContacted = false;
                m_currentTimeOnPcDate = DateTime.UtcNow.ToUniversalTime();
                m_currentTimeOnNtpDate = DateTimeNTP.GetNetworkTime().ToUniversalTime();
                m_currentTimeOnPc = m_currentTimeOnPcDate.ToString();
                m_currentTimeOnNtp = m_currentTimeOnNtpDate.ToString();
                m_currentTimeOnPcTick = m_currentTimeOnPcDate.Ticks;
                m_currentTimeOnNtpTick = m_currentTimeOnNtpDate.Ticks;
                m_differencePcNtpTick = m_currentTimeOnNtpTick - m_currentTimeOnPcTick;
                m_differencePcNtpMilliseconds = m_differencePcNtpTick / TimeSpan.TicksPerMillisecond;
                m_serverContacted = true;
            }
            catch (Exception ) { 
            
            }
        }

        public long GetCurrentTimestampTickServerNTP()
        {
            //return DateTime.UtcNow.ToUniversalTime().Ticks - m_differencePcNtpTick;
            return DateTime.UtcNow.ToUniversalTime().Ticks + m_differencePcNtpTick;
        }
    }

    [ContextMenu("Local Update From client")]
     void UpdateDifferenceWithServerNTPFromClient()
    {
        if (!isLocalPlayer) return;
        StartCoroutine(Coroutine_CheckForTimeDifferenceBetweenPlayerAndNTP(() => { }));
    }
    public void UpdateDifferenceWithServerNTPFromServerOnClient()
    {
        if (!isServer) return;
        RpcUpdateDifferenceWithServerNTP(m_serverToSyncOn);
    }

    [ClientRpc]
    private void RpcUpdateDifferenceWithServerNTP(string server) {

        m_serverToSyncOn = server;
        StartCoroutine(Coroutine_CheckForTimeDifferenceBetweenPlayerAndNTP(()=> {

            CmdPushServerDifferencetoServer(m_serverPcDifference.m_differencePcNtpMilliseconds);

        }));
    }

    private IEnumerator Coroutine_CheckForTimeDifferenceBetweenPlayerAndNTP(Action callback)
    {
        if (!isLocalPlayer) yield  break;
        
        while (!m_serverPcDifference.m_serverContacted)
        {
            m_serverPcDifference.Refresh();
            yield return new WaitForSeconds(1);

        }
        if(callback!=null)
            callback.Invoke();
    }

    [Command]
    private void CmdPushServerDifferencetoServer(double differencePcNtpMilliseconds)
    {
        m_timeBetweenPcServerNtp = differencePcNtpMilliseconds;
    }

    public long GetCurrentTimestampTickServerNTP()
    {
        return m_serverPcDifference.GetCurrentTimestampTickServerNTP();
    }
}

    

