using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputDirection.magnitude < 0.1f) return;
        Vector2 normalizedDirection = inputDirection.normalized; // Unsure if GetAxis is normalized already
        MoveThePlayer_RequestToServer_Rpc(normalizedDirection);
    }
    
    [Rpc(SendTo.Server, RequireOwnership = true)]
    private void MoveThePlayer_RequestToServer_Rpc(Vector2 inputDirection)
    {
        // Check if it's legal/not cheating
        MoveThePlayer_ServerToClients_Rpc(inputDirection);
        
        // Potentially handle client side movement here to make game feel smoother for non-host clients? Then correct discrepancies in server to clients?
    }

    [Rpc(SendTo.ClientsAndHost, RequireOwnership = false)]
    private void MoveThePlayer_ServerToClients_Rpc(Vector2 inputDirection)
    {
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
        
        float facingAngle = Vector3.SignedAngle(transform.forward, moveDirection.normalized, Vector3.up);
        
        if (facingAngle != 0)
        {
            rb.AddRelativeTorque(0, turnSpeed * (facingAngle / 180f), 0, ForceMode.Acceleration);
        }
    }
}