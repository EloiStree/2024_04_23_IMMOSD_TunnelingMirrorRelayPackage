using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class MirrorPlayerMono_RSAHandshake : NetworkBehaviour
{
    [SyncVar(hook = nameof(PlayerHandshakeStateChanged))]
    public byte m_handshakeState;
    public EnumMirrorRsaHankshakeServerSide m_handShakeAsEnum;


    public event System.Action<EnumMirrorRsaHankshakeServerSide> OnPlayerHandshakeStateChanged;

    public string m_subfolderMirrorRsaStorage = "MirrorKeyPair";
    public string m_messageToSign;

    [Header("Client")]
    public string m_client_publicKeySent;
    public string m_client_privateKey;
    public string m_client_messageSignedB64;

    [Header("Server")]
    public string m_server_publicKeyReceived;
    public string m_server_guidSent;
    public byte[] m_server_guidSentAsByte;
    public string m_server_b64SignedMessage;


    [SyncVar]
    public bool m_isHandshakeEstablished;


    public override void OnStartServer()
    {
        m_handshakeState = (byte)EnumMirrorRsaHankshakeServerSide.IsGuest;
    }


    public override void OnStartLocalPlayer()
    {
        if (!isLocalPlayer) return;
        GenerateAndReadKeyPairInPermaSubfolder.GetOrCreatePrivatePublicRsaKey(m_subfolderMirrorRsaStorage,
            out  m_client_privateKey, out m_client_publicKeySent, false);
        CmdSayHelloToServer(m_client_publicKeySent);
        Debug.Log("OnStartLocalPlayer");
    }


    [Command]
    public void CmdSayHelloToServer(string publicKeyRSA)
    {

        m_server_publicKeyReceived = publicKeyRSA;
        m_server_guidSent = Guid.NewGuid().ToString();
        m_server_guidSentAsByte = Encoding.UTF8.GetBytes(m_server_guidSent);
        RpcMessageToSign(m_server_guidSent);
        Debug.Log("CmdSayHelloToServer");
    }

    [ClientRpc]
    public void RpcMessageToSign(string messageToSign)
    {

        if (!isLocalPlayer) return;
        m_messageToSign = messageToSign;
        byte[] messageAsByte = Encoding.UTF8.GetBytes(messageToSign);
        byte[] signed = KeyPairRsaHolderToSignMessageUtility.SignData(messageAsByte, m_client_privateKey);

        m_client_messageSignedB64 = Convert.ToBase64String(signed);

        CmdPushSignedMessage(m_client_messageSignedB64);
        Debug.Log("RpcMessageToSign:"+messageToSign);
    }

    [Command]
    public void CmdPushSignedMessage(string signMessageAsB64)
    {
        m_server_b64SignedMessage = signMessageAsB64;
        byte[] signedbyte = Convert.FromBase64String(signMessageAsB64);
        m_isHandshakeEstablished= KeyPairRsaHolderToSignMessageUtility.VerifySignature(m_server_guidSentAsByte, signedbyte, m_server_publicKeyReceived);
        Debug.Log("CmdPushSignedMessage:" + signMessageAsB64);
    }

    void PlayerHandshakeStateChanged(byte _, byte handShakeState)
    {
        m_handShakeAsEnum = (EnumMirrorRsaHankshakeServerSide)handShakeState;
        OnPlayerHandshakeStateChanged?.Invoke(m_handShakeAsEnum);
    }
}


