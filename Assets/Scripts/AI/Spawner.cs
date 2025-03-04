using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3 spawnArea;
    [SerializeField] private bool spawnOnStart;
    [SerializeField] private int startSpawnAmount;
    
    private void Start()
    {
        if (prefab != null && spawnOnStart)
            Spawn(startSpawnAmount);
    }

    public void Spawn(int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
            Instantiate(prefab,
                transform.position - new Vector3(Random.Range(-spawnArea.x * 0.5f, spawnArea.x * 0.5f), 
                                                 Random.Range(-spawnArea.y * 0.5f, spawnArea.y * 0.5f), 
                                                 Random.Range(-spawnArea.z  * 0.5f, spawnArea.z * 0.5f)),
                Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }
}