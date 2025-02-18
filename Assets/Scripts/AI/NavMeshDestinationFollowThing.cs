using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDestinationFollowThing : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private KeyCode updatePathDebugKey = KeyCode.C;
    [SerializeField] private float turnStrength = 5f;
    private int targetCorner = 1;

    // NAVMESH
    // Variable that gets filled in with the points
    private NavMeshPath path;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // ONLY USE UPDATE WHILE DEVELOPING. Eventually your planner will call this only when it needs to
    private void Update()
    {
        CalculatePath(targetPoint.position); // Need to change this to plan once, then play through each saved corner
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        float angle = Vector3.SignedAngle(transform.forward, path.corners[targetCorner] - transform.position, Vector3.up);
        
        if (angle > 0)
        {
            rb.AddRelativeTorque(0, turnStrength, 0);
        }
        else
        {
            rb.AddRelativeTorque(0, -turnStrength, 0);
        }
    }

    private void CalculatePath(Vector3 target)
    {
        // Create it in Awake or something
        path = new NavMeshPath();

        // Call this when you want to go somewhere! Then read the path variable and you’ll see
        var calculatedPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

        for (var i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
        }

        targetCorner = 1;

        if (calculatedPath)
            return;
        // When you're INSIDE a valid NavMeshSurface, it'll find the closest edge
        NavMesh.FindClosestEdge(transform.position, out var hit, int.MaxValue);

        Debug.DrawRay(hit.position, transform.position - hit.position, Color.green);
    }
}