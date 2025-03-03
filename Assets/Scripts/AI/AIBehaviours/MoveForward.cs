using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveForce = 15f;
    [SerializeField] private float forwardRayDistance = 3f;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        bool obstructed = Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, forwardRayDistance);
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * forwardRayDistance, Color.red);
        float forceReduction = 0;
        if (obstructed)
        {
            forceReduction = (forwardRayDistance - hit.distance) * (moveForce / forwardRayDistance);
        }

        rb.AddForce(transform.forward * (moveForce - forceReduction));
    }
}
