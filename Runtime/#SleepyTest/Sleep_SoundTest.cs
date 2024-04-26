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

    public int m_secondModulo=4;
    public int m_previousSeconds;
    void Update()
    {
        
        int seconds = (m_useNtp?m_ntp.GetAdjustedTime().Second:DateTime.UtcNow.Second)% m_secondModulo;
        if (seconds != m_previousSeconds) {
            m_previousSeconds = seconds;
            if(seconds==0)
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
