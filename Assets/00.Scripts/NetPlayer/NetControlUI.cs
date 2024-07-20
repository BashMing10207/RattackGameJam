using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetControlUI : MonoBehaviour
{
    public Button host, client;

    public TMP_InputField CodeInput;
    public TextMeshProUGUI CodeOut;

    private void Awake()
    {   
        host.onClick.AddListener(() =>
        {
            CodeOut.text = NetGameMana.INSTANCE.relayMana.HostStartMing();
            //NetworkManager.Singleton.StartHost();

        });
        client.onClick.AddListener(() =>
        {
            NetGameMana.INSTANCE.relayMana.JoinMing(CodeInput.text);
            //NetworkManager.Singleton.StartClient();
        });
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
