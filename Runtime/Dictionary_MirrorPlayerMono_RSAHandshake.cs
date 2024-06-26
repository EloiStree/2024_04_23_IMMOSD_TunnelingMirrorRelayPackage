﻿using System.Collections.Generic;

public static class Dictionary_MirrorPlayerMono_RSAHandshake {

    public static Dictionary<string, RSAP_RSAHandshake> m_mirrorPlayerValide = new Dictionary<string, RSAP_RSAHandshake>();

    public static void Set(RSAP_RSAHandshake player) {
        if (player != null && player.IsPublicKeyValide()) {

            string key = player.GetPublicKey();
            if (m_mirrorPlayerValide.ContainsKey(key))
            {

                m_mirrorPlayerValide.Add(key, player);
            }
            else {
                m_mirrorPlayerValide[key] = player;
            }
        }
    }

    public static void RemoveEmptyOrInvalide() {

        foreach (var item in m_mirrorPlayerValide.Keys)
        {
            RSAP_RSAHandshake script = m_mirrorPlayerValide[item];
            if (script == null || !script.IsPublicKeyValide()) { 
                m_mirrorPlayerValide.Remove(item);
            }
        }
    
    }

    public static void Remove(RSAP_RSAHandshake handshake)
    {
        if (handshake == null)return;
        if (!handshake.IsPublicKeyValide()) return;

        string publicKey = handshake.GetPublicKey();
        if (m_mirrorPlayerValide.ContainsKey(publicKey)) 
            m_mirrorPlayerValide.Remove(publicKey);
    }
}


