using UnityEngine;

public class TurnTowards : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnStrength = 5f;
    
    public bool HasTarget { get; set; }
    public Vector3 TargetPosition { get; set; }
    
    private void Awake()
    {
        rb ??= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (!HasTarget) return;

        float angle = Vector3.SignedAngle(transform.forward, TargetPosition - transform.position, Vector3.up);

        if (angle != 0)
        {
            rb.AddRelativeTorque(0, turnStrength * (angle / 180f), 0);
        }
    }
}
