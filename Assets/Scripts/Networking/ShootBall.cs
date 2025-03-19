using Unity.Netcode;
using UnityEngine;

public class ShootBall : NetworkBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float shootVelocity;

    private void Update()
    {
        if (!IsLocalPlayer) return;
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall_RequestToServer_Rpc(transform.forward);
        }
    }
    
    [Rpc(SendTo.Server, RequireOwnership = true)]
    private void ShootBall_RequestToServer_Rpc(Vector3 shootDirection)
    {
        // Check if it's legal/not cheating
        ShootBall_ServerToClients_Rpc(shootDirection);
    }

    [Rpc(SendTo.ClientsAndHost, RequireOwnership = false)]
    private void ShootBall_ServerToClients_Rpc(Vector3 shootDirection)
    {
        if (!IsServer) return;
        
        GameObject ball = Instantiate(ballPrefab, transform.position + transform.forward, Quaternion.identity);
        
        ball.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

        ball.GetComponent<Rigidbody>().AddForce(shootDirection * shootVelocity, ForceMode.VelocityChange);
    }
}
