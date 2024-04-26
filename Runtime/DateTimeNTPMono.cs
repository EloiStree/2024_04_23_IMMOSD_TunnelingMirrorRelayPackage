using System;
using UnityEngine;

public class DateTimeNTPMono: MonoBehaviour
{
    public string m_ntpServer= "time.windows.com";
    public string m_currentTimeOnPC;
    public string m_currentTimeOnNtp;
    public DateTime m_currentTimeOnPCDate;
    public DateTime m_currentTimeOnNtpDate;

    private void Awake()
    {
        m_currentTimeOnPCDate = DateTime.UtcNow.ToUniversalTime();
        m_currentTimeOnNtpDate = DateTimeNTP.GetNetworkTime().ToUniversalTime();
        m_currentTimeOnPC = m_currentTimeOnPCDate.ToString();
        m_currentTimeOnNtp = m_currentTimeOnNtpDate.ToString();
    }
}
