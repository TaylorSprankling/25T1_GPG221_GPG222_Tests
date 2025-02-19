using System;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealthStuffs healthReference;
    [SerializeField] private TextMeshPro healthTMP;

    private void Awake()
    {
        if (healthReference == null)
        {
            healthReference = GetComponentInParent<PlayerHealthStuffs>();
        }

        if (healthTMP == null)
        {
            healthTMP = GetComponent<TextMeshPro>();
        }
    }

    private void OnEnable()
    {
        healthReference.health.OnValueChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        healthReference.health.OnValueChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(float previousValue, float newValue)
    {
        healthTMP.text = "Health: " + newValue.ToString("#.0");
    }
}
