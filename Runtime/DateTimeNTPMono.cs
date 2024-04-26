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
    public bool currentDateTimeUtcUseSummer;
    public bool currentDateTimeUtcNTPUseSummer;
    public DateTime GetAdjustedTime()
    {
        return DateTime.UtcNow.AddSeconds( m_differencePcNtpMilliseconds);
    }

    private void Awake()
    {
        TimeZoneInfo timeZone = TimeZoneInfo.Local;
        currentDateTimeUtcUseSummer = timeZone.IsDaylightSavingTime(DateTime.UtcNow);

        Refresh(); }

    [ContextMenu("Refresh")]
    public void Refresh() { 
        m_currentTimeOnPcDate = DateTime.UtcNow;
        m_currentTimeOnNtpDate = DateTimeNTP.GetNetworkTime();

        TimeZoneInfo timeZone = TimeZoneInfo.Local;
        currentDateTimeUtcUseSummer = timeZone.IsDaylightSavingTime(DateTime.UtcNow);
        currentDateTimeUtcNTPUseSummer = timeZone.IsDaylightSavingTime(m_currentTimeOnNtpDate);
        m_currentTimeOnPc = m_currentTimeOnPcDate.ToString();
        m_currentTimeOnNtp = m_currentTimeOnNtpDate.ToString();
        m_currentTimeOnPcTick =m_currentTimeOnPcDate.Ticks ;
        m_currentTimeOnNtpTick= m_currentTimeOnNtpDate.Ticks;
        m_differencePcNtpTick = m_currentTimeOnNtpTick - m_currentTimeOnPcTick;
        m_differencePcNtpMilliseconds = m_differencePcNtpTick /(double) TimeSpan.TicksPerMillisecond;

    }
}
