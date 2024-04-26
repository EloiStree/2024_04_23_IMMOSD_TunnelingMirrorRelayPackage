using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class TDD_RSAPlayerTimeOnServerNTPMono : NetworkBehaviour
{
    public RSAPlayerTimeOnServerNTPMono m_source;

    public UnityEvent m_callActionNtp;

    [System.Serializable]
    public struct DateTimeSyncPush {

        public long m_timeStampTickServerNTPSentRequest;
        public long m_tickBeforeExecuting;

    }

    public bool m_toExecuteNext;
    public long m_whenToExecute;


    [ContextMenu("Call Test from server")]
    public void PlayInOneSecondsCallFromServerOnly() {

        RtcPlaySoundOnClientInMilliseconds(m_source.GetCurrentTimestampTickServerNTP()+ (TimeSpan.TicksPerSecond));
    }

    [ClientRpc]
    public void RtcPlaySoundOnClientInMilliseconds(long whenToExecuteOnServerNtpTimestamp)
    {
        m_toExecuteNext = true;
           m_whenToExecute = whenToExecuteOnServerNtpTimestamp;
    }

    private void Update()
    {
        if (m_toExecuteNext && m_source.GetCurrentTimestampTickServerNTP() >= m_whenToExecute) {
            m_toExecuteNext = false;
            m_callActionNtp.Invoke();


        }
    }

}

    

