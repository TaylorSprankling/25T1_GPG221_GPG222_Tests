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
                    currentNeighbour.HCost = (currentNeighbour.WorldPosition - targetNode.WorldPosition).magnitude;
                    currentNeighbour.Parent ??= currentNode;
                    currentNeighbour.GCost = currentNeighbour.Parent.GCost + (currentNeighbour.WorldPosition - currentNeighbour.Parent.WorldPosition).magnitude;
                    if (currentNeighbour.FCost == 0f || currentNeighbour.HCost + currentNeighbour.GCost < currentNeighbour.FCost)
                    {
                        currentNeighbour.FCost = currentNeighbour.HCost + currentNeighbour.GCost;
                        currentNeighbour.Parent = currentNode;
                    }
                    open.Add(currentNeighbour);
                    if (currentNeighbour == targetNode) goto pathRetrace;
                }
            }

            open.Remove(currentNode);
            closed.Add(currentNode);
        }
        pathRetrace:
        finalPath.Add(targetNode);
        while (!finalPath.Contains(startNode))
        {
            finalPath.Add(finalPath[^1].Parent);
        }
        finalPath.Reverse();
    }
    
    private void OnDrawGizmos()
    {
        for (int x = 0; x < worldScanner.scanResolution.x; x++)
        {
            for (int z = 0; z < worldScanner.scanResolution.z; z++)
            {
                if (finalPath.Contains(worldScanner.GridNodeReferences[x, z])) // REALLY slow, but hey
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(new Vector3(x * worldScanner.pixelSize + worldScanner.pixelSize * 0.5f, 
                                                0, 
                                                z * worldScanner.pixelSize + worldScanner.pixelSize * 0.5f)  + worldScanner.transform.position, 
                                    Vector3.one * worldScanner.pixelSize);
                }
            }
        }
    }

}
