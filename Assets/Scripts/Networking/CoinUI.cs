using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private Inventory inventoryReference;
    [SerializeField] private TextMeshPro coinsTMP;

    private void Awake()
    {
        if (inventoryReference == null) inventoryReference = GetComponentInParent<Inventory>();
        if (coinsTMP == null) coinsTMP = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        UpdateUI(0, inventoryReference.coins.Value);
    }

    private void OnEnable()
    {
        inventoryReference.coins.OnValueChanged += UpdateUI;
    }

    private void OnDisable()
    {
        inventoryReference.coins.OnValueChanged -= UpdateUI;
    }

    private void UpdateUI(int previousValue, int newValue)
    {
        coinsTMP.text = "Coins: " + newValue;
    }
}