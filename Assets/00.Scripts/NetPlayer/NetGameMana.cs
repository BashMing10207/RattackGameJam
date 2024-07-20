using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGameMana: MonoBehaviour
{
    public bool isMultiplayer = true;

    static bool ISMUTIPLAYER;

    public static NetGameMana INSTANCE;
    
    public NetCPlayer player;
    public Player playerOff;
    public NetPool pool;
    public SkillManager skillManager;
    
    public TestLobby relayMana;
    private void Awake()
    {
        INSTANCE = this;
        ISMUTIPLAYER = isMultiplayer;
    }
    public static bool H_ISMULTI()//วฺ..ต้...?
    {
        return ISMUTIPLAYER;
    }
}
