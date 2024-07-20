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
        SizeAndMove(blackExtraLife, NetCPlayer.extraLifeCount[0].Value);
        SizeAndMove(whiteExtrLife, NetCPlayer.extraLifeCount[1].Value);
        SizeAndMove(blackLife, NetCPlayer.stones[0].Count);
        SizeAndMove(whiteLife, NetCPlayer.stones[1].Count);
    }
    private void Update()
    {
        ChangeLife();
    }
    public void ChangeLife()
    {
        SizeAndMove(blackExtraLife, NetCPlayer.extraLifeCount[0].Value);
        SizeAndMove(whiteExtrLife, NetCPlayer.extraLifeCount[1].Value);
        SizeAndMove(blackLife, NetCPlayer.stones[0].Count);
        SizeAndMove(whiteLife, NetCPlayer.stones[1].Count);
        
        print(NetCPlayer.extraLifeCount[1].Value);
    }


    void SizeAndMove(RawImage ming,int size)
    {
        float tmp = ming.uvRect.width;
        ming.uvRect = new Rect(0,0, size, 1);
        ming.uvRect.Set(0, 0, size, 1);
        ming.rectTransform.localPosition = (size*-25-700)*Vector3.right + Vector3.up* ming.rectTransform.localPosition.y;
        ming.rectTransform.sizeDelta = new Vector2(size * 50, 50);
    }
}
