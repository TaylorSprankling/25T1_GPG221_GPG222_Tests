using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinRelayUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject hostAndJoinUIPanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ManageRelay relay;
    [SerializeField] private TextMeshProUGUI joinCodeUI;

    private void Awake()
    {
        if (networkManager == null) networkManager = FindAnyObjectByType<NetworkManager>();
    }

    private void OnEnable()
    {
        networkManager.OnClientConnectedCallback += UpdateJoinCodeUI;
    }

    private void OnDisable()
    {
        networkManager.OnClientConnectedCallback -= UpdateJoinCodeUI;
    }

    public void JoinRelayButton()
    {
        relay.JoinRelay(inputField.text);
    }

    private void UpdateJoinCodeUI(ulong @ulong)
    {
        joinCodeUI.text = "Join code: " + relay.JoinCode;
        hostAndJoinUIPanel.SetActive(false);
    }
}
