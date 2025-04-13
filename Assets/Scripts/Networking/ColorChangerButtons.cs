using Unity.Netcode;
using UnityEngine;

public class ColorChangerButtons : MonoBehaviour
{
    public NetworkManager networkManager;
    private NetworkObject localPlayerObject;
    
    private void Awake()
    {
        if (networkManager == null) networkManager = FindAnyObjectByType<NetworkManager>();
    }
    
    private void OnEnable()
    {
        networkManager.OnClientConnectedCallback += GetThePlayer;
    }
    
    private void OnDisable()
    {
        networkManager.OnClientConnectedCallback -= GetThePlayer;
    }
    
    private void GetThePlayer(ulong @ulong)
    {
        if (localPlayerObject) return;
        localPlayerObject = networkManager.SpawnManager.GetLocalPlayerObject();
    }
    
    public void ChangeColor(int color)
    {
        if (!localPlayerObject) return;
        localPlayerObject.GetComponent<PlayerColor>().ChangeMyColor_ClientToServer_Rpc(color);
    }
}
