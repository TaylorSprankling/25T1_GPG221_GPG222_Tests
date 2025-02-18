using UnityEngine;

public class Wander : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float randomAmount = 10f;

    private void FixedUpdate()
    {
        TurnAround();
    }

    private void TurnAround()
    {
        var perlinNoise = Mathf.PerlinNoise1D(Time.time);
        perlinNoise += Random.Range(-randomAmount, randomAmount);
        rb.AddRelativeTorque(0, perlinNoise, 0);
    }
}
