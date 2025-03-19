using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(100f);

    public void Damage(float damage)
    {
        if (!IsServer) return;
        if (currentHealth.Value > damage)
        {
            currentHealth.Value -= damage;
        }
        else
        {
            currentHealth.Value = 0;
        }
    }

    public void Heal(float heal)
    {
        if (!IsServer) return;
        currentHealth.Value += heal;
    }
}
