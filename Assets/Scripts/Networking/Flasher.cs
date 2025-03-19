using Unity.Netcode;
using UnityEngine;

public class Flasher : NetworkBehaviour
{
    [SerializeField] private Light myLight;
    [SerializeField, Range(0, 100)] private int flashProbability = 0;

    private void Awake()
    {
        if (myLight == null)
        {
            myLight = GetComponent<Light>();
        }
    }

    private void FixedUpdate()
    {
        if (!IsServer) // This is a handy variable you can use to prevent code from running on the client
            return;
        if (Random.Range(0, 100) <= flashProbability)
        {
            // The !light.enabled bit is just how I’m toggling the bool back and forth. It means NOT
            ChangeLightStateClientRpc(!myLight.enabled); // Note I send through variables over the network. You can send multiple ones with commas, BUT you CAN’T send references to other objects as you normally would. There are other ways to do that though
        }
    }

    // This is the ‘attribute’ that tells Unity you want to network this function
    [Rpc(SendTo.ClientsAndHost, RequireOwnership = false)]
    private void ChangeLightStateClientRpc(bool state) // You MUST name it with “ClientRpc” at the end. I think just to make sure you know what you’re doing
    {
        myLight.enabled = state;
    }

}
