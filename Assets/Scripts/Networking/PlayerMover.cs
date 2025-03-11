using Unity.Netcode;
using UnityEngine;

public class PlayerMover : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        // Local only. Not networked
        if (IsLocalPlayer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveThePlayer_RequestToServer_Rpc(KeyCode.W);
            }
        }
    }


    // Function that ONLY runs on the server. Typically for client controller code when they press buttons etc
    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void MoveThePlayer_RequestToServer_Rpc(KeyCode keyCode)
    {
        // Check if it's legal/not cheating
        MoveThePlayer_ServerToClients_Rpc(keyCode);
    }


    // Function that runs from the Server TO ALL clients
    [Rpc(SendTo.ClientsAndHost, RequireOwnership = false)]
    private void MoveThePlayer_ServerToClients_Rpc(KeyCode keyCode)
    {
        // This is bugged
        rb.AddForce(0, 0, 100);
    }
}