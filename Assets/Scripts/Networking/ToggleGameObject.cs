using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] canvasGroups;
    private bool toggledStatus = true;

    public void ToggleObject()
    {
        if (toggledStatus)
        {
            foreach (var cg in canvasGroups)
            {
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            
            toggledStatus = false;
        }
        else
        {
            foreach (var cg in canvasGroups)
            {
                cg.alpha = 1;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            
            toggledStatus = true;
        }
    }
}
