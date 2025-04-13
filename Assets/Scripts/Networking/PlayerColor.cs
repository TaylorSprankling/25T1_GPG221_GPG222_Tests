using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    public Material[] colorMaterials;
    public Renderer myRenderer;
    [HideInInspector] public NetworkVariable<int> thisPlayerColor = new NetworkVariable<int>(0);
    
    public override void OnNetworkSpawn()
    {
        myRenderer.material = colorMaterials[thisPlayerColor.Value];
    }
    
    [Rpc(SendTo.Server, RequireOwnership = true)]
    public void ChangeMyColor_ClientToServer_Rpc(int color)
    {
        ChangeMaterial_ServerToClients_Rpc(color);
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeMaterial_ServerToClients_Rpc(int materialIndex)
    {
        myRenderer.material = colorMaterials[materialIndex];
        thisPlayerColor.Value = materialIndex;
    }
}
