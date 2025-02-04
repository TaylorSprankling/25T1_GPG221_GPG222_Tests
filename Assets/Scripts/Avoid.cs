using System;
using UnityEngine;

public class Avoid : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float distance = 4;
    [SerializeField] private float turnSpeed = 15;

    private void FixedUpdate()
    {
        var hitSomething = Physics.Raycast(transform.position + (Vector3.up * 0.5f), transform.forward, out RaycastHit hit, distance);

        if (hitSomething)
        {
            rb.AddRelativeTorque(0, turnSpeed, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.5f), transform.forward * distance);
    }
}
