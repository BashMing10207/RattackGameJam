using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetTransport : UnityTransport
{
    public static NetTransport instance;

    private void Awake()
    {
        instance = this; 
    }
}
