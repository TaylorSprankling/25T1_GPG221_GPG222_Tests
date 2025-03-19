using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageCollider : NetworkBehaviour
{
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private List<Material> myMaterials;
    [SerializeField] private float damage;
    [Tooltip("If set to 0, will destroy the object on collision")]
    [SerializeField] private float downtime;
    [SerializeField] private NetworkVariable<bool> canDamage = new (true);

    private void Start()
    {
        myRenderer.material = canDamage.Value ? myMaterials[0] : myMaterials[1];
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        if (!canDamage.Value) return;
        
        Health healthRef = other.gameObject.GetComponent<Health>();

        if (healthRef == null) return;
        
        healthRef.Damage(damage);
        
        if (downtime <= 0)
        {
            gameObject.GetComponent<NetworkObject>().Despawn(true);
            return;
        }
        canDamage.Value = false;
        ChangeMaterial_ServerToClients_Rpc(1);
        StartCoroutine(ManageDowntime());
    }
    
    [Rpc(SendTo.ClientsAndHost, RequireOwnership = false)]
    private void ChangeMaterial_ServerToClients_Rpc(int materialIndex)
    {
        myRenderer.material = myMaterials[materialIndex];
    }
    
    private IEnumerator ManageDowntime()
    {
        if (!NetworkManager.Singleton.IsServer) yield break;
        yield return new WaitForSeconds(downtime);
        ChangeMaterial_ServerToClients_Rpc(0);
        canDamage.Value = true;
    }
}
