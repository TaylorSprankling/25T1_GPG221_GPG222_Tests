using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPathFollower : MonoBehaviour
{
    [SerializeField] private WorldScanner worldScanner;
    [SerializeField] private Transform target;
    private Node targetNode;
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();
    private List<Node> finalPath = new List<Node>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CalculatePath();
        }
    }

    private void CalculatePath()
    {
        Vector3 relativeGridPosition = ((transform.position - worldScanner.transform.position) -
                                        new Vector3(worldScanner.pixelSize * 0.5f, 0, worldScanner.pixelSize * 0.5f)) / worldScanner.pixelSize;
        int relativeGridPosX = Mathf.RoundToInt(relativeGridPosition.x);
        int relativeGridPosZ = Mathf.RoundToInt(relativeGridPosition.z);
        Node startNode = worldScanner.GridNodeReferences[relativeGridPosX, relativeGridPosZ];
        open.Add(startNode); // Starting node
        
        Vector3 targetGridPosition = ((target.transform.position - worldScanner.transform.position) -
                                      new Vector3(worldScanner.pixelSize * 0.5f, 0, worldScanner.pixelSize * 0.5f)) / worldScanner.pixelSize;
        int targetGridPosX = Mathf.RoundToInt(targetGridPosition.x);
        int targetGridPosZ = Mathf.RoundToInt(targetGridPosition.z);
        targetNode = worldScanner.GridNodeReferences[targetGridPosX, targetGridPosZ];
        while (open.Count > 0)
        {
            Node currentNode = open[Random.Range(0, open.Count)];
            for (int i = 0; i < open.Count; i++)
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
                    if (currentNode.GridPositionX + xOffset < 0 || currentNode.GridPositionX + xOffset >= worldScanner.scanResolution.x ||
                        currentNode.GridPositionZ + zOffset < 0 || currentNode.GridPositionZ + zOffset >= worldScanner.scanResolution.z)
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
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (Node n in open)
        {
            Gizmos.DrawCube(new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y), new Vector3(worldScanner.pixelSize, 0.002f, worldScanner.pixelSize));
        }
        
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        foreach (Node n in closed)
        {
            Gizmos.DrawCube(new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y), new Vector3(worldScanner.pixelSize, 0.002f, worldScanner.pixelSize));
        }
        
        Gizmos.color = Color.blue;
        foreach (Node n in finalPath)
        {
            Gizmos.DrawCube(new Vector3(n.WorldPosition.x, 0, n.WorldPosition.y), new Vector3(worldScanner.pixelSize, 0.002f, worldScanner.pixelSize));
        }
    }
}
