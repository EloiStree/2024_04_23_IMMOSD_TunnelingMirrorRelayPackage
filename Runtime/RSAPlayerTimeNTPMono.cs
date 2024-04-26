using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSAPlayerTimeNTPMono : NetworkBehaviour
{
    //https://youtu.be/BAo5C2qbLq8?t=238-

    public NTPLongKey m_timeDifference;



    [SyncVar]
    public double m_pingSentReceived;

    [SyncVar]

    public double m_pingSentReceivedSeconds;
    [SyncVar]
    public long m_differenceInTick;






    [ContextMenu("Refresh Timing from Server")]
    public void RefreshTimingFromServerCall()
    {

        RpcRefreshTiming();
    }
    [ContextMenu("Refresh Timing from Client")]
    public void RefreshTimingFromClientCall()
    {
        StartExchangeNTP();
        
    }

    [ClientRpc]
    public void RpcRefreshTiming()
    {
        StartExchangeNTP();
    }
    public void StartExchangeNTP()
    {
        NTPLongKey timePack = new NTPLongKey();
        timePack.m_t0_dateTimeClientSent = TicksLocalMachineUTC();
        CmdPushTimeStart(timePack);
    }


    [Command]
    public void CmdPushTimeStart(NTPLongKey time)
    {
       time. m_t1_dateTimeGenerateServerStart = TicksLocalMachineUTC();
       time. m_t2_dateTimeGenerateServerSent = TicksLocalMachineUTC();
        RpcNotifyClientThatTimeIsReceived(time);
    }
    [ClientRpc]
    public void RpcNotifyClientThatTimeIsReceived(NTPLongKey time)
    {
        time.m_t3_dateTimeClientReceived = TicksLocalMachineUTC();
        CmdPushTimeReceived(time);
        //OnClient
        ComputeDifferenceInTick(time);
    }
    [Command]
    public void CmdPushTimeReceived(NTPLongKey time)
    {
        //OnServer
        ComputeDifferenceInTick(time);
    }

    

    private void ComputeDifferenceInTick(NTPLongKey time)
    {
        m_timeDifference = time;
        m_pingSentReceived = time.m_t3_dateTimeClientReceived - time.m_t0_dateTimeClientSent;
        m_differenceInTick =  
            (time.m_t3_dateTimeClientReceived - time.m_t0_dateTimeClientSent)
            - (time.m_t2_dateTimeGenerateServerSent - time.m_t1_dateTimeGenerateServerStart);
        m_pingSentReceivedSeconds = m_differenceInTick /(double) TimeSpan.TicksPerMillisecond;
    }

    private static long TicksLocalMachineUTC()
    {
        return DateTime.UtcNow.Ticks;
    }
    [System.Serializable]
    public struct NTPLongKey {
        public long m_t0_dateTimeClientSent;
        public long m_t1_dateTimeGenerateServerStart;
        public long m_t2_dateTimeGenerateServerSent;
        public long m_t3_dateTimeClientReceived;
    }
}
