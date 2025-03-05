using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomPathCalculator : MonoBehaviour, IPathingCalculator
{
    [SerializeField] private WorldScanner worldScanner;
    private List<Node> open;
    private List<Node> closed;
    private List<Node> finalPath;

    public event Action<Vector3[]> NewPathCalculated;

    private void Awake()
    {
        worldScanner ??= FindFirstObjectByType<WorldScanner>();
    }

    private void OnEnable()
    {
        MouseTarget.RightClickNewTargetPosition += CalculatePath;
    }

    private void OnDisable()
    {
        MouseTarget.RightClickNewTargetPosition -= CalculatePath;
    }

    private void CalculatePath(Vector3 target)
    {
        open = new List<Node>();
        closed = new List<Node>();
        finalPath = new List<Node>();
        Vector3 targetGridPosition = ((target - worldScanner.transform.position) -
                                      new Vector3(worldScanner.pixelSize * 0.5f, 0, worldScanner.pixelSize * 0.5f)) / worldScanner.pixelSize;
        int targetGridPosX = Mathf.RoundToInt(targetGridPosition.x);
        int targetGridPosZ = Mathf.RoundToInt(targetGridPosition.z);
        if (worldScanner.GridNodeReferences[targetGridPosX, targetGridPosZ] == null)
        {
            return;
        }
        Node targetNode = worldScanner.GridNodeReferences[targetGridPosX, targetGridPosZ];
        if (targetNode.IsBlocked)
        {
            return; // Change this to look for closest node that isn't blocked
        }
        
        Vector3 relativeGridPosition = ((transform.position - worldScanner.transform.position) -
                                        new Vector3(worldScanner.pixelSize * 0.5f, 0, worldScanner.pixelSize * 0.5f)) / worldScanner.pixelSize;
        int relativeGridPosX = Mathf.RoundToInt(relativeGridPosition.x);
        int relativeGridPosZ = Mathf.RoundToInt(relativeGridPosition.z);
        Node startNode = worldScanner.GridNodeReferences[relativeGridPosX, relativeGridPosZ];
        open.Add(startNode); // Starting node
        while (open.Count > 0)
        {
            Node currentNode = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].FCost < currentNode.FCost)
                {
                    currentNode = open[i];
                }
            }
            open.Remove(currentNode);
            closed.Add(currentNode);
            if (currentNode == targetNode) break;
            for (int xOffset = -1; xOffset < 2; xOffset++)
            {
                for (int zOffset = -1; zOffset < 2; zOffset++)
                {
                    if (xOffset == 0 && zOffset == 0) continue;
                    if (currentNode.GridPositionX + xOffset < 0 || currentNode.GridPositionX + xOffset >= worldScanner.ScanResolution.x ||
                        currentNode.GridPositionZ + zOffset < 0 || currentNode.GridPositionZ + zOffset >= worldScanner.ScanResolution.z)
                    {
                        continue;
                    }
                    
                    Node currentNeighbour = worldScanner.GridNodeReferences[currentNode.GridPositionX + xOffset, currentNode.GridPositionZ + zOffset];
                    if (currentNeighbour.IsBlocked || closed.Contains(currentNeighbour)) continue;
                    if (currentNeighbour.HCost == -1)
                    {
                        currentNeighbour.HCost = Mathf.RoundToInt(Vector2.Distance(currentNeighbour.WorldPosition, targetNode.WorldPosition) * 1000);
                    }
                    int gCostCalc = currentNode.GCost + Mathf.RoundToInt(Vector2.Distance(currentNeighbour.WorldPosition, currentNode.WorldPosition) * 1000);
                    if (currentNeighbour.Parent == null)
                    {
                        currentNeighbour.Parent = currentNode;
                        currentNeighbour.GCost = gCostCalc;
                        currentNeighbour.FCost = currentNeighbour.HCost + gCostCalc;
                    }
                    else if (currentNeighbour.HCost + gCostCalc < currentNeighbour.FCost)
                    {
                        currentNeighbour.Parent = currentNode;
                        currentNeighbour.GCost = gCostCalc;
                        currentNeighbour.FCost = currentNeighbour.HCost + gCostCalc;
                    }
                    if (!open.Contains(currentNeighbour))
                    {
                        open.Add(currentNeighbour);
                    }
                }
            }
        }

        finalPath.Add(targetNode);
        while (!finalPath.Contains(startNode))
        {
            finalPath.Add(finalPath[^1].Parent);
        }

        finalPath.Reverse();

        foreach (Node n in open)
        {
            n.Reset();
        }

        foreach (Node n in closed)
        {
            n.Reset();
        }

        Vector3[] waypoints = new Vector3[finalPath.Count];
        for (int index = 0; index < finalPath.Count; index++)
        {
            Node n = finalPath[index];
            waypoints[index] = new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y);
        }

        NewPathCalculated?.Invoke(waypoints);
    }

    private void OnDrawGizmos()
    {
        if (finalPath == null || !DebugToggles.DrawCalculatedPaths) return;

        Gizmos.color = Color.blue;
        foreach (Node n in finalPath)
        {
            Gizmos.DrawCube(new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y), new Vector3(worldScanner.pixelSize, 0.002f, worldScanner.pixelSize));
        }

        Gizmos.color = Color.green;
        foreach (Node n in open)
        {
            Gizmos.DrawCube(new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y), new Vector3(worldScanner.pixelSize, 0.002f, worldScanner.pixelSize));
        }

        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        foreach (Node n in closed)
        {
            if (finalPath.Contains(n)) continue;
            Gizmos.DrawCube(new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y), new Vector3(worldScanner.pixelSize, 0.002f, worldScanner.pixelSize));
        }
    }
}
