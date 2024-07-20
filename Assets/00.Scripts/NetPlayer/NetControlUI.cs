using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetControlUI : MonoBehaviour
{
    public Button host, client;
    public static NetControlUI INSTANCE;

    public TMP_InputField CodeInput;
    public TextMeshProUGUI CodeOut;

    public GameObject roomUI;
    

    
    string text;
    private void Awake()
    {
        INSTANCE = this;

        host.onClick.AddListener(() =>
        {
            NetGameMana.Instance.relayMana.HostStartMing();
            
            //Invoke(nameof(DelayOut), 1f);
            //NetworkManager.Singleton.StartHost();

        });
        client.onClick.AddListener(() =>
        {
            NetGameMana.Instance.relayMana.JoinMing(CodeInput.text);
            //NetworkManager.Singleton.StartClient();
        });
    }

    public void OnJoin(string code)
    {
        CodeOut.text = code;
        roomUI.SetActive(false);
        
        
    }

    void DelayOut()
    {
        CodeOut.text = text;
    }
    //public void EventHost()
    //{
    //    // Clear the default listen endpoints.
    //    NetTransport.instance..Clear();
        
    //    // Adds your own endpoint
    //    UnetTransport.ServerTransports.Add(new TransportHost()
    //    {
    //        Name = "My UDP Socket",
    //        Port = int.Parse(port.text.Trim()),
    //        WebSockets = false
    //    });
    //    NetworkingManager.singleton.StartHost();
    //}
}
