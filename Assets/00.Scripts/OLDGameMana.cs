
using UnityEngine;

public class OLDGameMana : MonoBehaviour
{
    public static OLDGameMana instance;

    public Player player;
    public OLDPool pool;

    private void Awake()
    {
        instance = this;
    }

    //public static bool IsMulti()
    //{
    //    return (OLDGameMana.instance == null);
    //}
}
