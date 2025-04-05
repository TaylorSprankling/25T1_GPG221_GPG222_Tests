using UnityEngine;

public class Wander : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float randomAmount = 1f;

    private void FixedUpdate()
    {
        TurnAround();
    }

    private void TurnAround()
    {
        // float perlinNoise = Mathf.PerlinNoise1D(Time.time);
        // perlinNoise += Random.Range(-randomAmount, randomAmount);
        // rb.AddRelativeTorque(0, perlinNoise, 0);
        rb.AddRelativeTorque(0, Random.Range(-randomAmount, randomAmount), 0);
    }
}