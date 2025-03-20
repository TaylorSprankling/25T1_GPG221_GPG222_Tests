using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ManageRelay : MonoBehaviour
{
    public string JoinCode { get; private set; }
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            Debug.Log(joinCode);

            JoinCode = joinCode;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4,
                                                                                     (ushort)allocation.RelayServer.Port,
                                                                                     allocation.AllocationIdBytes,
                                                                                     allocation.Key,
                                                                                     allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            JoinCode = joinCode.ToUpper();
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(joinAllocation.RelayServer.IpV4,
                                                                                       (ushort)joinAllocation.RelayServer.Port,
                                                                                       joinAllocation.AllocationIdBytes,
                                                                                       joinAllocation.Key,
                                                                                       joinAllocation.ConnectionData,
                                                                                       joinAllocation.HostConnectionData);
            
            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
