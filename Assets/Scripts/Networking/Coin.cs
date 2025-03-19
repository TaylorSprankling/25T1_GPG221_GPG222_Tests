using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        
        Inventory invRef = other.GetComponent<Inventory>();

        if (invRef == null) return;
        
        invRef.AddCoins(1);
        
        gameObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
