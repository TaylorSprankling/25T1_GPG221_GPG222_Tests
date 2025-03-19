using Unity.Netcode;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int spawnAmount;
    [SerializeField] private Vector3 spawnArea;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!NetworkManager.Singleton.IsServer) return;
        
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject cube = Instantiate(objectPrefab, transform.position +
                                          new Vector3(Random.Range(-spawnArea.x, spawnArea.x),
                                                      Random.Range(-spawnArea.y, spawnArea.y),
                                                      Random.Range(-spawnArea.z, spawnArea.z)),
                                                      Quaternion.identity);
            cube.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}
