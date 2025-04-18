using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField] [Range(0f, 100f)] private float visionDistance = 25f;
    [SerializeField] [Range(0.001f, 360f)] private float visionFOV = 90f;
    [SerializeField] private List<ObjectInVision> targets;
    
    [Header("Debug")]
    [SerializeField] private bool drawVisionWireCone;
    [SerializeField] private bool drawOnlyOnSelection = true;
    [SerializeField] private Color wireConeColor = Color.white;
    
    public List<ObjectInVision> Targets => targets;
    
    private void Start() // hillbilly find targets
    {
        GameObject[] targetsInScene = GameObject.FindGameObjectsWithTag("Target");
        
        foreach (GameObject target in targetsInScene)
        {
            AddNewTarget(target);
        }
    }
    
    public void AddNewTarget(GameObject newTarget)
    {
        ObjectInVision inVisionClass = new() { objectReference = newTarget };
        targets.Add(inVisionClass);
    }
    
    private void Update()
    {
        HandleVisionToTarget();
    }
    
    private void HandleVisionToTarget()
    {
        if (targets.Count <= 0) return;
        
        foreach (ObjectInVision target in targets.ToList())
        {
            if (!target.objectReference)
            {
                targets.Remove(target);
                continue;
            }
            
            // If target is out of range, target is not in vision
            if ((target.objectReference.transform.position - transform.position).magnitude > visionDistance)
            {
                target.isInVision = false;
                continue;
            }
            
            // Check relative angle to forward face
            float angle = Vector3.Angle(transform.forward, target.objectReference.transform.position - transform.position);
            
            // If target is outside field of view, target is not in vision
            if (angle > visionFOV / 2f)
            {
                target.isInVision = false;
                continue;
            }
            
            Physics.Linecast(transform.position, target.objectReference.transform.position, out RaycastHit hit); // Check for obstruction
            
            target.isInVision = hit.transform == target.objectReference.transform; // If unobstructed, target is in vision
            
            // Opted to not use raycast to check distance due to it checking distance to collider
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!drawVisionWireCone || drawOnlyOnSelection) return;
        DrawTheVisionWireCone();
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!drawVisionWireCone || !drawOnlyOnSelection) return;
        DrawTheVisionWireCone();
    }
    
    private void DrawTheVisionWireCone()
    {
        Gizmos.color = wireConeColor;
        GizmosExtensions.DrawWireArc(transform.position, transform.forward, visionFOV, visionDistance);
    }
}