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
    public int m_receivedDeciSecondsTotale;
    public int m_previousDeciSeconds;
    public int m_currentDeciSeconds;
    public int m_colorIndex;

    public Color[] m_colorArray= new Color[] { Color.red * 0.2f, Color.green * 0.2f, Color.blue * 0.2f};

    public bool m_useMainCameraDebug=true;

    void Update()
    {
        m_receivedDeciSecondsTotale =(int)( (m_useNtp ? m_ntp.GetAdjustedTime().Ticks : DateTime.UtcNow.Ticks) / (TimeSpan.TicksPerSecond/10));
        m_currentDeciSeconds = (int) ((m_receivedDeciSecondsTotale) % m_deciSeconds);
        if (m_currentDeciSeconds != m_previousDeciSeconds) {
            m_previousDeciSeconds = m_currentDeciSeconds;
            if(m_currentDeciSeconds==0)
                Tick(m_receivedDeciSecondsTotale);
        }
    }

    private void Tick(int value)
    {
        m_colorIndex = (Math.Abs(value) / m_deciSeconds) % m_colorArray.Length;
        m_onTick.Invoke();
        Color c = m_colorArray[m_colorIndex];
        m_onTickColor.Invoke(c);
        if (m_useMainCameraDebug && Camera.main)
            Camera.main.backgroundColor = c;
    }
}
