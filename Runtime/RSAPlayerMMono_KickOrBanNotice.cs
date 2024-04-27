using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RSAPlayerMMono_KickOrBanNotice : NetworkBehaviour
{

    [Tooltip("Player is kick out because there is not enough space, he is a guest and a valide RSA player arrived")]
    public UnityEvent<string> m_onGuestKickOut;
    [Tooltip("Player/User did something wrong and is disconnected as a warning")]
    public UnityEvent<string> m_onWarningKickOut;
    [Tooltip("Player/User did something wrong and is unallowed until a datetime as a warning")]
    public UnityEvent<string> m_onTemporalBan;
    [Tooltip("Player/User did something wrong and is in the ban list from now on")]
    public UnityEvent<string> m_onPermaBan;



    [ContextMenu("Push Random Message")]
    public void PushRandomMessageFromServer() {

        if (!isServer) {
            Debug.Log("Not server", this.gameObject);
            return;
        }
        int value=  UnityEngine.Random.Range(0, 5);
        string msg = Guid.NewGuid().ToString();
        switch (value) {
               case 0: RpcNoticeInComingKickOutOutOfSpace(msg);              return;
               case 1: RpcNoticeInComingKickOutWarning(msg);                 return;
               case 2: RpcNoticeInComingBanTime(msg,0 ,0,"-");               return;
               case 3: RpcNoticeInComingPermaBan("InsertRsaKey", msg, "-");  return;
               case 4: RpcNoticeConnectionRefuse("InsertRsaKey", msg, "-"); return;
            default: break;
        }
    }

    public bool m_useDebugLog;
    [ClientRpc]
    void RpcNoticeInComingKickOutOutOfSpace(string message)
    {

        if (m_useDebugLog)
            Debug.Log("Kick out for space: " + message);
    }
    [ClientRpc]
    void RpcNoticeInComingKickOutWarning(string message)
    {

        if (m_useDebugLog)
            Debug.Log("Kick out warning: " + message);
    }
    [ClientRpc]
    void RpcNoticeInComingBanTime(string message, long serverDateTimeBanNTPFrom, long serverDateTimeBanNTPTo, string guidBanLogId)
    {

        if (m_useDebugLog)
            Debug.Log("Temporal Ban: " + message);
    }
    [ClientRpc]
    void RpcNoticeInComingPermaBan(string rsaPermaBanned, string message, string guidBanLogId)
    {

        if (m_useDebugLog)
            Debug.Log("Perma ban: " + message);
    }
    [ClientRpc]
    void RpcNoticeConnectionRefuse(string rsaPermaBanned, string message, string guidBanLogId)
    {
        if (m_useDebugLog)
            Debug.Log("connection refused: " + message);

    }
}
