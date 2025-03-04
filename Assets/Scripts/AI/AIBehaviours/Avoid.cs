using UnityEngine;

public class Avoid : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private int feelersPerSide = 2;
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private bool ignoreWalls;

    private float wallAngle;
    private bool leavingDeadEnd;

    public bool IgnoreWalls { get => ignoreWalls; set => ignoreWalls = value; }

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleFeelers();
    }
    
    private void HandleFeelers()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        for (int i = 1; i < feelersPerSide + 1; i++)
        {
            Vector3 rightRayDirection = Quaternion.AngleAxis((fieldOfView * 0.5f) * (i / (float)feelersPerSide), transform.up) * transform.forward;
            bool rightRayHit = Physics.Raycast(rayOrigin, rightRayDirection, out RaycastHit rightHit, maxDistance);
            Debug.DrawRay(rayOrigin, rightRayDirection * maxDistance, Color.red);
            
            Vector3 leftRayDirection = Quaternion.AngleAxis((-fieldOfView * 0.5f) * (i / (float)feelersPerSide), transform.up) * transform.forward;
            bool leftRayHit = Physics.Raycast(rayOrigin, leftRayDirection, out RaycastHit leftHit, maxDistance);
            Debug.DrawRay(rayOrigin, leftRayDirection * maxDistance, Color.red);

            if (rightRayHit && leftRayHit)
            {
                if (!leavingDeadEnd)
                {
                    Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, maxDistance);
                    wallAngle = Vector3.SignedAngle(transform.forward, -hit.normal, -Vector3.up);
                    leavingDeadEnd = true;
                }

                switch (wallAngle)
                {
                    case < 0:
                        rb.AddRelativeTorque(0, -turnSpeed, 0);
                        break;
                    default:
                        rb.AddRelativeTorque(0, turnSpeed, 0);
                        break;
                }

                return;
            }

            leavingDeadEnd = false;

            switch (rightRayHit)
            {
                case true when IgnoreWalls && rightHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"): // Need to do something to ignore STATIC walls when attempting to follow path
                    break;
                case true:
                    rb.AddRelativeTorque(0, -turnSpeed / feelersPerSide * ((maxDistance - rightHit.distance) / maxDistance), 0);
                    break;
            }

            switch (leftRayHit)
            {
                case true when IgnoreWalls && leftHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"):
                    break;
                case true:
                    rb.AddRelativeTorque(0, turnSpeed / feelersPerSide * ((maxDistance - rightHit.distance) / maxDistance), 0);
                    break;
            }
        }
    }
}