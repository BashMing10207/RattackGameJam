using UnityEngine;

public class NetGameMana: MonoBehaviour
{
    public bool isMultiplayer = true;

    static bool ISMUTIPLAYER;

    public static NetGameMana Instance;
    
    public NetCPlayer player;
    public Player playerOff;
    public NetPool pool;
    public SkillManager skillManager;
    
    public LifeUI LifeUI;
    
    public GameObject playerHand;
    public CardSelectPanel CardSelectPanel;
    
    public TestLobby relayMana;

    public GameObject win, lose;

    //SkillManaV2 skillManaV2;
    private void Awake()
    {
        Instance = this;
        ISMUTIPLAYER = isMultiplayer;
        
    }
    public static bool H_ISMULTI()//핸..들...?
    {
        return ISMUTIPLAYER;
    }
}
