using Mirror;
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

    [ClientRpc]
    void RpcNoticeInComingKickOutOutOfSpace(string message)
    {

    }
    [ClientRpc]
    void RpcNoticeInComingKickOutWarning(string message)
    {

    }
    [ClientRpc]
    void RpcNoticeInComingBanTime(string message, long serverDateTimeBanNTPFrom, long serverDateTimeBanNTPTo, string guidBanLogId)
    {

    }
    [ClientRpc]
    void RpcNoticeInComingPermaBan(string rsaPermaBanned, string message, string guidBanLogId)
    {

    }
    [ClientRpc]
    void RpcNoticeConnectionRefuse(string rsaPermaBanned, string message, string guidBanLogId)
    {

    }
}
