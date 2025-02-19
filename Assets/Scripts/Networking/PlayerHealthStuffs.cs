using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealthStuffs : NetworkBehaviour
{
    public NetworkVariable<float> health = new NetworkVariable<float>(100f);

    // Will make a damager and healer trigger object to test between different players
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(1.5f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(2.25f);
        }
    }

    public void Damage(float damage)
    {
        if (!IsServer)
        {
            return;
        }
        if (health.Value > damage)
        {
            health.Value -= damage;
        }
        else
        {
            health.Value = 0;
        }
    }

    public void Heal(float heal)
    {
        if (!IsServer)
        {
            return;
        }
        health.Value += heal;
    }
}
