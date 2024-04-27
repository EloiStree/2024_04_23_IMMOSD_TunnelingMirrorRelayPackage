using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushIntegerToMirrorInstanceMono : MonoBehaviour
{
    public void PushInteger(int integerValue)
    {
        RSAP_PushIntegerToServer i = RSAP_PushIntegerToServer.GetInstanceLocalPlayer();
        if (i != null)
        {
            i.PushInteger(integerValue);
        }
    }
    [ContextMenu("Push Random")]
    public void PushIntegerRandomInteger()
    {
        RSAP_PushIntegerToServer i = RSAP_PushIntegerToServer.GetInstanceLocalPlayer();
        if (i != null)
        {
            i.PushIntegerRandom();
        }
    }

    [ContextMenu("Push Min")]
    public void PushIntegerMin() => PushInteger(int.MinValue);
    [ContextMenu("Push Max")]
    public void PushIntegerMax() => PushInteger(int.MaxValue);
    [ContextMenu("Push Zero")]
    public void PushIntegerZero() => PushInteger(0);

}
