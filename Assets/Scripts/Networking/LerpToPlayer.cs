using Unity.Netcode;
using UnityEngine;

public class LerpToPlayer : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private NetworkObject localPlayerObject;
    [SerializeField] private float speed;

    private void Awake()
    {
        if (networkManager == null) networkManager = FindAnyObjectByType<NetworkManager>();
    }

    private void OnEnable()
    {
        networkManager.OnClientConnectedCallback += GetThePlayer;
    }
    
    private void OnDisable()
    {
        networkManager.OnClientConnectedCallback -= GetThePlayer;
    }
    
    private void GetThePlayer(ulong @ulong)
    {
        if (localPlayerObject) return;
        localPlayerObject = networkManager.SpawnManager.GetLocalPlayerObject();
    }

    private void FixedUpdate()
    {
        if (!localPlayerObject) return;
        gameObject.transform.position = Vector3.Lerp(transform.position, localPlayerObject.transform.position, Time.fixedDeltaTime * speed); ;
    }
}
