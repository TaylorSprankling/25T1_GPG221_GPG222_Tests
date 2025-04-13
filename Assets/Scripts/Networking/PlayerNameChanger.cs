using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class PlayerNameChanger : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    
    public void ChangePlayerName()
    {
        _ = ChangePlayerNameTask();
    }
    
    private async Task ChangePlayerNameTask()
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(inputField.text);
        Debug.Log("Player name is now: " + AuthenticationService.Instance.PlayerName);
    }
}
