using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OwnerPlayerInvoker : NetworkBehaviour
{

    void Update()
    {
        if (IsOwner)
        {
            NetGameMana.INSTANCE.player.Updeat();
        }
    }
}