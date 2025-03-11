using System.Collections.Generic;
using UnityEngine;

public class Align : MonoBehaviour
{
    // Variable pointing to your Neighbours component
    // Neighbours neighbours;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Neighbours neighbours;
    [SerializeField] private float force = 3f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (neighbours == null) neighbours = GetComponent<Neighbours>();
    }

    private void FixedUpdate()
    {
        // Some are Torque, some are Force		
        Vector3 targetDirection = CalculateMove(neighbours.List);
        
        if (DebugToggles.DrawAlignRays)
        {
            // Where I WANT to face
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, targetDirection * 3f, Color.blue);
            // Where I'm facing right now
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * 3f, Color.white);
        }
        
        // Cross will take YOUR direction and the TARGET direction and turn it into a rotation force vector. It CROSSES through both at 90 degrees
        Vector3 cross = Vector3.Cross(transform.forward, targetDirection);

        rb.AddTorque(cross * force);
    }

    private Vector3 CalculateMove(List<GameObject> neighboursList)
    {
        if (neighboursList.Count == 0)
            return Vector3.zero;

        Vector3 alignmentDirection = Vector3.zero;

        // Average of all neighbours directions
        // I’m using a list of transforms in my neighbours script, you might be using GameObjects etc
        foreach (GameObject item in neighboursList)
        {
            alignmentDirection+= item.transform.forward;
        }

        alignmentDirection/= neighboursList.Count;

        return alignmentDirection;
    }
}
