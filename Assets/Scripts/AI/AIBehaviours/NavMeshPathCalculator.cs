using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPathCalculator : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnStrength = 10f;
    [Tooltip("Distance from targeted corner when AI will set target to next corner")]
    [SerializeField] private float targetSwapDistance = 1f;
    
    private NavMeshPath path; // NavMesh variable that gets filled in with the points
    private int targetCorner = 1;
    
    public event Action NewTargetSet;
    public event Action TargetReached;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void OnEnable()
    {
        MouseTarget.LeftClickNewTargetPosition += CalculatePath;
    }

    private void OnDisable()
    {
        MouseTarget.LeftClickNewTargetPosition -= CalculatePath;
    }

    private void FixedUpdate()
    {
        FollowPath(); // Put this in align behaviour when made
    }

    private void CalculatePath(Vector3 target)
    {
        path = new NavMeshPath();

        // Call this when you want to go somewhere! Then read the path variable and you’ll see
        bool calculatedPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
        
        switch (calculatedPath)
        {
            case true:
            {
                targetCorner = 1;
                NewTargetSet?.Invoke();
                break;
            }
            case false:
                path = null;
                // When you're INSIDE a valid NavMeshSurface, it'll find the closest edge
                NavMesh.FindClosestEdge(transform.position, out NavMeshHit hit, int.MaxValue);
                break;
        }
    }

    private void FollowPath()
    {
        if (path == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, path.corners[targetCorner]) <= targetSwapDistance)
        {
            targetCorner++;
        }

        if (targetCorner == path.corners.Length)
        {
            TargetReached?.Invoke();
            path = null;
            return;
        }
        
        float angle = Vector3.SignedAngle(transform.forward, path.corners[targetCorner] - transform.position, Vector3.up);
        
        if (angle != 0)
        {
            rb.AddRelativeTorque(0, turnStrength * (angle / 180f), 0);
        }
    }

    private void OnDrawGizmos()
    {
        if (path == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < path.corners.Length - targetCorner; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawLine(transform.position, path.corners[targetCorner]);
            }
            else
            {
                Gizmos.DrawLine(path.corners[i + targetCorner - 1], path.corners[i + targetCorner]);
            }
        }
    }
}