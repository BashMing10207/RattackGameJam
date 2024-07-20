using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetGameMana:MonoBehaviour
{
    public bool isMultiplayer = true;

    static bool ISMUTIPLAYER;

    public static NetGameMana Instance;
    
    public NetCPlayer player;
    public Player playerOff;
    public NetPool pool;
    public SkillManager skillManager;
    
    public TestLobby relayMana;

    public LifeUI lifeUI;
    private void Awake()
    {
        //if (IsOwner)
        
        Instance = this;
        ISMUTIPLAYER = isMultiplayer;
        
    }
    public static bool H_ISMULTI()//핸..들...?
    {
        return ISMUTIPLAYER;
    }
}
