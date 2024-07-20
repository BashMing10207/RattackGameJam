using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class TestLobby : MonoBehaviour
{
    public static string CODE;

    void Awake()
    {
        NetGameMana.INSTANCE.relayMana = this;
    }
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => { };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    
    public void HostStartMing()
    {
        CreateRelay();
    }

    public void JoinMing(string id)
    {
        JoinRelay(id);
    }

    async void CreateRelay()
    {
        try
        {
        Allocation allocation =  await RelayService.Instance.CreateAllocationAsync(2);

            CODE = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );
            NetworkManager.Singleton.StartHost();
        }
        catch(RelayServiceException e)
        {
            Debug.LogException(e);
        }
    }

    async void JoinRelay(string joinCode)
    {
        try
        {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData
                (
                            joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                            joinAllocation.AllocationIdBytes,
                            joinAllocation.Key,
                            joinAllocation.ConnectionData,
                            joinAllocation.HostConnectionData
                );
            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e)
        {
            Debug.LogException(e);
        }
    }

}
