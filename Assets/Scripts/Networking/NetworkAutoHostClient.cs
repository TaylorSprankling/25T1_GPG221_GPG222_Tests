using Unity.Netcode;
using UnityEngine;

public class NetworkAutoHostClient : MonoBehaviour
{
    #if UNITY_EDITOR
    private void Start()
    {
        if (ParrelSync.ClonesManager.IsClone())
        {
            NetworkManager.Singleton.StartClient();
        }
        else if (!ParrelSync.ClonesManager.IsClone())
        {
            NetworkManager.Singleton.StartHost();
        }
    }
    #endif
}
