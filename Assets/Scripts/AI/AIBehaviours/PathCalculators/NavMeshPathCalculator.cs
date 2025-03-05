using System;
using UnityEngine;
using UnityEngine.AI;

public interface IPathingCalculator
{
    public event Action<Vector3[]> NewPathCalculated;
}

public class NavMeshPathCalculator : MonoBehaviour, IPathingCalculator
{
    private NavMeshPath path; // NavMesh variable that gets filled in with the points

    public event Action<Vector3[]> NewPathCalculated;

    private void OnEnable()
    {
        MouseTarget.LeftClickNewTargetPosition += CalculatePath;
    }

    private void OnDisable()
    {
        MouseTarget.LeftClickNewTargetPosition -= CalculatePath;
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
                NewPathCalculated?.Invoke(path.corners);
                break;
            }
            case false:
                path = null;
                // When you're INSIDE a valid NavMeshSurface, it'll find the closest edge
                NavMesh.FindClosestEdge(transform.position, out NavMeshHit hit, int.MaxValue);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (path == null || !DebugToggles.DrawCalculatedPaths) return;
        Gizmos.color = Color.blue;
        for (int i = 1; i < path.corners.Length; i++)
        {
            Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);
        }
    }
}