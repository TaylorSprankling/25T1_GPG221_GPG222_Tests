using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 moveForce;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(moveForce);
    }
}
