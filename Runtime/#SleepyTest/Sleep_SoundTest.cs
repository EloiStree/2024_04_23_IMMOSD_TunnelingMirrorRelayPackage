using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sleep_SoundTest : MonoBehaviour
{

    public DateTimeNTPMono m_ntp;

    public bool m_useNtp;
    public UnityEvent m_onTick;
    public UnityEvent<Color> m_onTickColor;

    public int m_deciSeconds=50;
    public int m_deciseconds;
    public int m_previousDeciSeconds;
    public int m_currentDeciSeconds;
    void Update()
    {
        m_deciseconds =(int)( (m_useNtp ? m_ntp.GetAdjustedTime().Ticks : DateTime.UtcNow.Ticks) / (TimeSpan.TicksPerSecond/10));
         m_currentDeciSeconds = (int) ((m_deciseconds) % m_deciSeconds);
        if (m_currentDeciSeconds != m_previousDeciSeconds) {
            m_previousDeciSeconds = m_currentDeciSeconds;
            if(m_currentDeciSeconds == 0)
                Tick();
        }
    }

    private void Tick()
    {
        m_onTick.Invoke();
        Color c  = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        m_onTickColor.Invoke(c);
        if (Camera.main)
            Camera.main.backgroundColor = c;
    }
}
