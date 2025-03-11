using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveForce = 15f;
    [SerializeField] private float forwardRayDistance = 3f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Check if immediate path is obstructed
        bool obstructed = Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, forwardRayDistance);
        float forceReduction = 0;

        if (obstructed)
        {
            forceReduction = (forwardRayDistance - hit.distance) * (moveForce / forwardRayDistance);

            if (DebugToggles.DrawRays)
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * forwardRayDistance, new Color(1f, 1f, 0, .75f));
            }
        }
        else if (DebugToggles.DrawRays)
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * forwardRayDistance, new Color(1f, 1f, 1f, .5f));
        }

        rb.AddForce(transform.forward * (moveForce - forceReduction));
    }
}