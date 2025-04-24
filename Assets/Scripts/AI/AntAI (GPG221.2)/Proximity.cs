using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float inProximityDistance = 2f;
    [SerializeField] private Collider[] collidersInProximity;
    
    public Collider[] CollidersInProximity => collidersInProximity;
    
    [Header("Debug")]
    [SerializeField] private bool drawProximityWireSphere;
    [SerializeField] private bool drawOnlyOnSelection = true;
    [SerializeField] private Color wireConeColor = Color.white;
    
    private void Update()
    {
        CheckProximity();
    }
    
    private void CheckProximity()
    {
        collidersInProximity = Physics.OverlapSphere(transform.position, inProximityDistance);
    }
    
    private void OnDrawGizmos()
    {
        if (!drawProximityWireSphere || drawOnlyOnSelection) return;
        DrawWireSphere();
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!drawProximityWireSphere || !drawOnlyOnSelection) return;
        DrawWireSphere();
    }
    
    private void DrawWireSphere()
    {
        Gizmos.color = wireConeColor;
        Gizmos.DrawWireSphere(transform.position, inProximityDistance);
    }
}
