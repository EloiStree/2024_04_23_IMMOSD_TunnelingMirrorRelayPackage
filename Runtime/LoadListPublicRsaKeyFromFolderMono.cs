using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadListPublicRsaKeyFromFolderMono : MonoBehaviour
{

    public RSAPlayerGroupListMono m_toLoadIn;
    public void PushIn(string [] filePath)
    {
        if (filePath == null)
            return;

        foreach (var item in filePath)
        {
            if (item!=null && File.Exists(item)) {

                try
                {
                    m_toLoadIn.SetOrAdd(File.ReadAllText(item), true);
                }
                catch (Exception) {
                    Debug.Log("A file did not read correctly");
                }
            }
        }
    }

}
