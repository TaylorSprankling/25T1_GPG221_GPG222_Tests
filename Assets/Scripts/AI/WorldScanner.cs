using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WorldScanner : MonoBehaviour
{
    private Node[,] gridNodeReferences;
    [SerializeField] private Vector3 worldScanSize;
    [SerializeField] private float pixelSize = 1f;
    [SerializeField] private LayerMask detectedLayers;
    [SerializeField] private KeyCode scanKeyDebug;
    private Vector3 scanResolution;
    
    private void Awake()
    {
        ScanWorld();
    }

#if (UNITY_EDITOR)
    private void Update()
    {
        if (Input.GetKeyDown(scanKeyDebug))
        {
            ScanWorld();
        }
    }
#endif

    private void ScanWorld()
    {
        scanResolution = worldScanSize / pixelSize;
        gridNodeReferences = new Node[(int)(scanResolution.x), (int)(scanResolution.z)];
        for (int x = 0; x < scanResolution.x; x++)
        {
            for (int z = 0; z < scanResolution.z; z++)
            {
                gridNodeReferences[x, z] = new Node();

                if (Physics.CheckBox(new Vector3(
                            transform.position.x - (worldScanSize.x * 0.5f) + (x * pixelSize) + (pixelSize * 0.5f), 
                            transform.position.y, 
                            transform.position.z - (worldScanSize.z * 0.5f) + (z * pixelSize) + (pixelSize * 0.5f)), 
                        Vector3.one * (pixelSize * 0.5f), Quaternion.identity, detectedLayers))
                {
                    gridNodeReferences[x, z].IsBlocked = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (gridNodeReferences == null)
        {
            return;
        }
        for (int x = 0; x < scanResolution.x; x++)
        {
            for (int z = 0; z < scanResolution.z; z++)
            {
                if (gridNodeReferences[x, z].IsBlocked)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(new Vector3(
                            transform.position.x - (worldScanSize.x * 0.5f) + (x * pixelSize) + (pixelSize * 0.5f), 
                            transform.position.y, 
                            transform.position.z - (worldScanSize.z * 0.5f) + (z * pixelSize) + (pixelSize * 0.5f)), 
                        Vector3.one * pixelSize);
                }
            }
        }
    }
}

public class Node
{
    public bool IsBlocked;
    public float GCost;
    public float HCost;
    public float FCost;
}