using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
public class RSAPlayerMMono : NetworkBehaviour
    {
        // Events that the PlayerUI will subscribe to
        public event System.Action<byte> OnPlayerNumberChanged;
        public event System.Action<Color32> OnPlayerColorChanged;
        public event System.Action<byte> OnPlayerHandshakeStateChanged;
        public event System.Action<string> OnPlayePublicRsaKeyChanged;

    // Players List to manage playerNumber
    static readonly List<RSAPlayerMMono> playersList = new List<RSAPlayerMMono>();

        [Header("Player UI")]
        public GameObject playerUIPrefab;

        GameObject playerUIObject;

        #region SyncVars

        [Header("SyncVars")]

        /// <summary>
        /// This is appended to the player name text, e.g. "Player 01"
        /// </summary>
        [SyncVar(hook = nameof(PlayerNumberChanged))]
        public byte m_playerNumber = 0;


    [SyncVar(hook = nameof(PlayerPublicRsaKeyChanged))]

        public string m_playerPublicRsaKey = "";
      


    // This is called by the hook of playerNumber SyncVar above
    void PlayerNumberChanged(byte _, byte newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }  // This is called by the hook of playerNumber SyncVar above
    void PlayerPublicRsaKeyChanged(string _, string newRsaKey)
    {
        OnPlayePublicRsaKeyChanged?.Invoke(newRsaKey);
    }

        // This is called by the hook of playerNumber SyncVar above
        

    #endregion

    #region Server

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
        {
            base.OnStartServer();

            playersList.Add(this);



            InvokeRepeating(nameof(UpdateData), 1, 1);
             m_playerPublicRsaKey = "";
        }

        [ServerCallback]
        internal static void ResetPlayerNumbers()
        {
            byte playerNumber = 0;
            foreach (RSAPlayerMMono player in playersList)
                player.m_playerNumber = playerNumber++;
        }

       
        [ServerCallback]
        void UpdateData()
        {
            //playerData = (ushort)Random.Range(100, 1000);
        }

        [ServerCallback]
        void UpdateKeyPairServerRules(string keyPairServerRules)
        {

        }
        [ServerCallback]
        void UpdateIntegerInputServerManual(string manualText)
        {

        }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer()
        {
            CancelInvoke();
            playersList.Remove(this);
        }

        #endregion

        #region Client

        /// <summary>
        /// Called on every NetworkBehaviour when it is activated on a client.
        /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
        /// </summary>
        public override void OnStartClient()
        {
            //// Instantiate the player UI as child of the Players Panel
            //playerUIObject = Instantiate(playerUIPrefab, CanvasUI.GetPlayersPanel());
            //playerUI = playerUIObject.GetComponent<PlayerUI>();

            //// wire up all events to handlers in PlayerUI
            //OnPlayerNumberChanged = playerUI.OnPlayerNumberChanged;
            //OnPlayerColorChanged = playerUI.OnPlayerColorChanged;
            //OnPlayerDataChanged = playerUI.OnPlayerDataChanged;

            // Invoke all event handlers with the initial data from spawn payload
            OnPlayerNumberChanged?.Invoke(m_playerNumber);
            OnPlayePublicRsaKeyChanged?.Invoke(m_playerPublicRsaKey);
        //  OnPlayerDataChanged.Invoke(playerData);

        
        }
        public string m_guidSendToClientFromServer;

        /// <summary>
        /// Called when the local player object has been set up.
        /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            //// Set isLocalPlayer for this Player in UI for background shading
            //playerUI.SetLocalPlayer();

            //// Activate the main panel
            //CanvasUI.SetActive(true);
        }

        /// <summary>
        /// Called when the local player object is being stopped.
        /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStopLocalPlayer()
        {
            // Disable the main panel for local player
           // CanvasUI.SetActive(false);
        }

        /// <summary>
        /// This is invoked on clients when the server has caused this object to be destroyed.
        /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
        /// </summary>
        public override void OnStopClient()
        {
            // disconnect event handlers
            OnPlayerNumberChanged = null;
            OnPlayerColorChanged = null;
        }

        #endregion
    }

