using UnityEngine;
using UnityEngine.AI;

public class NavMeshDestinationFollowThing : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;

    [SerializeField] private KeyCode updatePathDebugKey = KeyCode.C;

    // NAVMESH
    // Variable that gets filled in with the points
    private NavMeshPath path;

    // ONLY USE UPDATE WHILE DEVELOPING. Eventually your planner will call this only when it needs to
    private void Update()
    {
        CalculatePath(targetPoint.position);
    }

    private void FixedUpdate() { }

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

        if (calculatedPath)
            return;
        // When you're INSIDE a valid NavMeshSurface, it'll find the closest edge
        NavMesh.FindClosestEdge(transform.position, out var hit, int.MaxValue);

        Debug.DrawRay(hit.position, transform.position - hit.position, Color.green);
    }
}