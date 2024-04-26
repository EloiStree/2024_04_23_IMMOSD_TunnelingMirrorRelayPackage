using System;
using UnityEngine;

public class DateTimeNTPMono: MonoBehaviour
{
    public string m_ntpServer= "time.windows.com";
    public string m_currentTimeOnPc;
    public string m_currentTimeOnNtp;
    public long m_currentTimeOnPcTick;
    public long m_currentTimeOnNtpTick;
    public long m_differencePcNtpTick;
    public double m_differencePcNtpMilliseconds;
    public DateTime m_currentTimeOnPcDate;
    public DateTime m_currentTimeOnNtpDate;

    private void Awake()
    { Refresh(); }

    [ContextMenu("Refresh")]
    public void Refresh() { 
        m_currentTimeOnPcDate = DateTime.UtcNow.ToUniversalTime();
        m_currentTimeOnNtpDate = DateTimeNTP.GetNetworkTime().ToUniversalTime();
        m_currentTimeOnPc = m_currentTimeOnPcDate.ToString();
        m_currentTimeOnNtp = m_currentTimeOnNtpDate.ToString();
        m_currentTimeOnPcTick =m_currentTimeOnPcDate.Ticks ;
        m_currentTimeOnNtpTick= m_currentTimeOnNtpDate.Ticks;
        m_differencePcNtpTick = m_currentTimeOnNtpTick - m_currentTimeOnPcTick;
        m_differencePcNtpMilliseconds = m_differencePcNtpTick / TimeSpan.TicksPerMillisecond;

    }
}
