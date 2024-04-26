using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RSAPlayerGroupListMono : MonoBehaviour
{

    public RSAPlayerGroupList m_data;

    [ContextMenu("Remove Double")]
    public void RemoveDouble()
    {

        m_data.RemoveDouble();
    }



    public void SetOrAdd(string publicXmlRSa)
    {
        m_data.SetOrAdd(publicXmlRSa, false);
    }
 
    public void SetOrAdd(string publicXmlRSa, bool checkForDouble)
    {
        m_data.SetOrAdd(publicXmlRSa, checkForDouble);

    }
}


[System.Serializable]
public class RSAPlayerGroupList
{

    public List<string> m_publicXmlRsaKey1024= new List<string>();


    [ContextMenu("Remove Double")]
    public void RemoveDouble() {

        m_publicXmlRsaKey1024 = m_publicXmlRsaKey1024.Distinct().ToList();
        //
    }

    public void SetOrAdd(string publicXmlRSa, bool checkForDouble)
    {
        m_publicXmlRsaKey1024.Add(publicXmlRSa);
        if(checkForDouble)
            RemoveDouble();

    }

}


[System.Serializable]
public class RSAPlayerGroupDico
{

    public Dictionary<string, bool> m_handshakeDico = new Dictionary<string, bool>();
    public string[] m_handshakeList = new string[0];

    public void SetOrAddPublicKeyXml(string rsaPublicKeyXml)
    {
        if (!m_handshakeDico.ContainsKey(rsaPublicKeyXml))
        {
            m_handshakeDico.Add(rsaPublicKeyXml, false);
            m_handshakeList = m_handshakeDico.Keys.ToArray();
        }
    }

    public void RemovePublicKeyXml(string rsaPublicKeyXml)
    {
        if (!m_handshakeDico.ContainsKey(rsaPublicKeyXml))
        {
            m_handshakeDico.Remove(rsaPublicKeyXml);
            m_handshakeList = m_handshakeDico.Keys.ToArray();
        }
    }

    public bool IsInDico(string publicKey)
    {
       return m_handshakeDico.ContainsKey(publicKey);
    }
}
