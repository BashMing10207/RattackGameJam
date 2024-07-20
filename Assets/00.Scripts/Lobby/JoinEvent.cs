using Unity.Netcode;
using UnityEngine;

public class JoinEvent :  MonoBehaviour
{
    public static GameObject INSTANCE;
    private void Awake()
    {
        INSTANCE = gameObject;
    }
}
