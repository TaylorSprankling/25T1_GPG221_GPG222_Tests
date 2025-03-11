using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Neighbours : MonoBehaviour
{
    [SerializeField] private float neighbourRadius;
    [SerializeField] private List<GameObject> neighboursList;
    [SerializeField] private List<GameObject> neighboursInLos;
    
    public float NeighbourRadius => neighbourRadius;
    public List<GameObject> List => neighboursInLos;

    private void Update()
    {
        // Check for neighbours using the Agent layer
        Collider[] neighboursArray = Physics.OverlapSphere(transform.position, neighbourRadius, 1 << LayerMask.NameToLayer("Agent"));
        neighboursList = new List<GameObject>();

        // Convert collider array to list of game objects
        foreach (Collider n in neighboursArray)
        {
            if (n.gameObject == gameObject) continue;
            neighboursList.Add(n.gameObject);
        }
        
        // Check for line of sight and add to neighbours in LOS list if applicable
        foreach (GameObject n in neighboursList)
        {
            Physics.Raycast(transform.position + transform.up * 0.5f, n.transform.position - transform.position, out RaycastHit hit);
            
            if (hit.collider.gameObject == n)
            {
                if (DebugToggles.DrawNeighbourRays)
                {
                    Debug.DrawRay(transform.position + transform.up * 0.5f, n.transform.position - transform.position, Color.white);
                }
                
                if (!neighboursInLos.Contains(n.gameObject))
                {
                    neighboursInLos.Add(n.gameObject);
                }
            }
            else if (hit.collider.gameObject != n)
            {
                if (DebugToggles.DrawNeighbourRays)
                {
                    Debug.DrawRay(transform.position + transform.up * 0.5f, n.transform.position - transform.position, Color.red);
                }

                if (neighboursInLos.Contains(n.gameObject))
                {
                    neighboursInLos.Remove(n.gameObject);
                }
            }
        }
        
        // Remove any game object not in the neighbours list as they are outside the neighbour radius now
        foreach (GameObject n in neighboursInLos.ToList())
        {
            if (!neighboursList.Contains(n))
            {
                neighboursInLos.Remove(n);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!DebugToggles.DrawNeighbourSphere) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, neighbourRadius);
    }
}