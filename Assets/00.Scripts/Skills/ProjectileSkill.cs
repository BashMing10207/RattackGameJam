using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public class ProjectileSkill : Skill
{
    protected override void AccleateSKilll(NetPlayerStone netPlayerStone, Vector3 forceInput, float magnitude)
    {
        
    }

    [ServerRpc]
    public void MingExpendServerRpc()
    {

    }
}
