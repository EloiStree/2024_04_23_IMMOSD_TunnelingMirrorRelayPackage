using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSAPlayerPermissionMono : MonoBehaviour
{
    public RSAPlayerGroupListMono m_allowedAsSource;
    public RSAPlayerGroupListMono m_allowedAsListener;
    public RSAPlayerGroupDico m_allowedAsSourceDico;
    public RSAPlayerGroupDico m_allowedAsListenerDico;



    public bool IsUserAllowsAsSource(string publicKey)
    {
        return m_allowedAsSourceDico.IsInDico(publicKey);
    }
    public bool IsUserAllowsAsListener(string publicKey)
    {
        return m_allowedAsSourceDico.IsInDico(publicKey);
    }

    public void Awake()
    {
        RefreshDictionary();
    }


    public void RefreshDictionary() {

        foreach (var item in m_allowedAsSource.m_data.m_publicXmlRsaKey1024)
        {
            m_allowedAsSourceDico.SetOrAddPublicKeyXml(item);
        }
        foreach (var item in m_allowedAsListener.m_data.m_publicXmlRsaKey1024)
        {
            m_allowedAsListenerDico.SetOrAddPublicKeyXml(item);
        }
    }
    
}
