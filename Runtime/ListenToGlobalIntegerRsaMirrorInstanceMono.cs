using UnityEngine;
using UnityEngine.Events;

public class ListenToGlobalIntegerRsaMirrorInstanceMono : MonoBehaviour {


    private static ListenToGlobalIntegerRsaMirrorInstanceMono m_instance;


    public static ListenToGlobalIntegerRsaMirrorInstanceMono GetInstance() {

        return m_instance;
    }

    public IntegerRequest m_onIntegerRequestDelegate;
    public UnityEvent<int, RSAP_RSAHandshake> m_onIntegerRequestEvent;
    public delegate void IntegerRequest(int value, in RSAP_RSAHandshake handshake);

    public int m_lastInteger;
    public RSAP_RSAHandshake m_lastHandshakeSource;

    public void Awake()
    {
        m_instance = this;
    }

    private void OnDestroy()
    {
        m_instance = null;
    }

    public void PushIntegerRequest(int newValue, RSAP_RSAHandshake handshake) {

        m_lastInteger = newValue;
        m_lastHandshakeSource = handshake;
        if (m_onIntegerRequestDelegate != null)
            m_onIntegerRequestDelegate(newValue, handshake);
        m_onIntegerRequestEvent.Invoke(newValue, handshake); 
    }
}
