using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    public NetworkVariable<int> coins = new(0);

    public void AddCoins(int amount)
    {
        if (!IsServer) return;

        coins.Value += amount;
    }
}