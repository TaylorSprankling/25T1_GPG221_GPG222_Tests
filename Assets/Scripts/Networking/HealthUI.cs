using System;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health healthReference;
    [SerializeField] private TextMeshPro healthTMP;

    private void Awake()
    {
        if (healthReference == null)
        {
            healthReference = GetComponentInParent<Health>();
        }

        if (healthTMP == null)
        {
            healthTMP = GetComponent<TextMeshPro>();
        }
    }

    private void Start()
    {
        UpdateHealthUI(0, healthReference.currentHealth.Value);
    }

    private void OnEnable()
    {
        healthReference.currentHealth.OnValueChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        healthReference.currentHealth.OnValueChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(float previousValue, float newValue)
    {
        healthTMP.text = "Health: " + newValue.ToString("#.0");
    }
}
