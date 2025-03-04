using UnityEngine;

public class WorldScanner : MonoBehaviour
{
    [SerializeField] public Vector3 worldScanSize;
    [SerializeField] public float pixelSize = 1f;
    [SerializeField] private LayerMask detectedLayers;

    public Node[,] GridNodeReferences;
    private Vector3 scannedPosition;
    private Vector3 scannedSize;
    public Vector3 scanResolution;

    [Header("Debug Options")]
    [SerializeField] private bool showScanArea;
    [SerializeField] private bool showObstacles;
    [SerializeField] private bool showFreeSpace;
    
    private void Awake()
    {
        ScanWorld();
    }

    public void ScanWorld()
    {
        if (worldScanSize.x % pixelSize != 0 || worldScanSize.z % pixelSize != 0)
        {
            #if UNITYEDITOR
            Debug.LogError("World scan size not divisible by pixel size");
            #endif
            return;
        }

        scanResolution = worldScanSize / pixelSize;
        GridNodeReferences = new Node[(int)scanResolution.x, (int)scanResolution.z];

        for (int x = 0; x < scanResolution.x; x++)
        {
            for (int z = 0; z < scanResolution.z; z++)
            {
                GridNodeReferences[x, z] = new Node
                {
                    GridPositionX = x,
                    GridPositionZ = z,
                    WorldPosition = new Vector2(transform.position.x + x * pixelSize + pixelSize * 0.5f,
                                                transform.position.z + z * pixelSize + pixelSize * 0.5f)
                };

                if (Physics.CheckBox(new Vector3(transform.position.x + x * pixelSize + pixelSize * 0.5f,
                                                 transform.position.y,
                                                 transform.position.z + z * pixelSize + pixelSize * 0.5f),
                                     Vector3.one * (pixelSize * 0.5f), Quaternion.identity, detectedLayers))
                {
                    GridNodeReferences[x, z].IsBlocked = true;
                }
            }
        }

        scannedPosition = transform.position;
        scannedSize = worldScanSize;
    }

    private void OnDrawGizmosSelected()
    {
        if (GridNodeReferences == null || transform.position != scannedPosition || worldScanSize != scannedSize)
        {
            if (!showScanArea) return;
            Gizmos.color = new Color(1, 0.5f, 0);
            Gizmos.DrawCube(transform.position + worldScanSize * 0.5f, new Vector3(worldScanSize.x, worldScanSize.y + 0.001f, worldScanSize.z));
            return;
        }

        if (showScanArea)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + worldScanSize * 0.5f, new Vector3(worldScanSize.x, worldScanSize.y + 0.001f, worldScanSize.z));
        }

        for (int x = 0; x < scanResolution.x; x++)
        {
            for (int z = 0; z < scanResolution.z; z++)
            {
                switch (GridNodeReferences[x, z].IsBlocked)
                {
                    case false when showFreeSpace:
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(new Vector3(transform.position.x + x * pixelSize + pixelSize * 0.5f,
                                                    transform.position.y,
                                                    transform.position.z + z * pixelSize + pixelSize * 0.5f),
                                        new Vector3(pixelSize, 0.001f, pixelSize));
                        break;
                    case true when showObstacles:
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(new Vector3(transform.position.x + x * pixelSize + pixelSize * 0.5f,
                                                    transform.position.y,
                                                    transform.position.z + z * pixelSize + pixelSize * 0.5f),
                                        new Vector3(pixelSize, 0.001f, pixelSize));
                        break;
                }
            }
        }
    }
}

public class Node
{
    public Node Parent;
    public Vector2 WorldPosition;
    public int GridPositionX;
    public int GridPositionZ;
    [Tooltip("gCost + hCost")] public int FCost;
    [Tooltip("Distance from parent")] public int GCost;
    [Tooltip("Distance to target")] public int HCost = -1;
    public bool IsBlocked;

    public void Reset()
    {
    Parent = null;
    FCost = 0;
    GCost = 0;
    HCost = -1;
    }
}