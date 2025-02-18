using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Avoid : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int maxRays = 1;
    [SerializeField] private float fieldOfView = 0;
    [SerializeField] private float maxDistance = 4;
    [SerializeField] private float turnSpeed = 15;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < maxRays; i++)
        {
            var rayOrigin = transform.position + (Vector3.up * 0.5f);
            var rayDirection = transform.forward;
            if (i >= 1)
            {
                
            }
            var hitSomething = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, maxDistance);
            Debug.DrawRay(rayOrigin, rayDirection * maxDistance);
            if (hitSomething)
            {
                rb.AddRelativeTorque(0, turnSpeed, 0);
            }
        }
    }
}
