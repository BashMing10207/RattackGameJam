using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : NetworkBehaviour
{
    [SerializeField]
    RawImage whiteExtrLife, blackExtraLife, whiteLife, blackLife;
    private void Awake()
    {
        NetGameMana.Instance.LifeUI = this;
    }
    [ServerRpc]
    public void ChangeLifeServerRpc()
    {
        SizeAndMove(blackExtraLife, NetCPlayer.extraLifeCount[0]);
        SizeAndMove(blackExtraLife, NetCPlayer.extraLifeCount[1]);
        SizeAndMove(blackLife, NetCPlayer.stones[0].Count);
        SizeAndMove(whiteLife, NetCPlayer.stones[1].Count);
    }
    public void ChangeLife()
    {
        if(NetCPlayer.extraLifeCount.Count>0)
        {
        SizeAndMove(blackExtraLife, NetCPlayer.extraLifeCount[0]);
        SizeAndMove(blackExtraLife, NetCPlayer.extraLifeCount[1]);
        SizeAndMove(blackLife, NetCPlayer.stones[0].Count);
        SizeAndMove(whiteLife, NetCPlayer.stones[1].Count);
        }
    }


    void SizeAndMove(RawImage ming,int size)
    {
        float tmp = ming.uvRect.width;
        ming.uvRect.Set(0,0,size,1);
        ming.rectTransform.position = ming.rectTransform.position + (size - tmp)*25*Vector3.right;
    }
}
