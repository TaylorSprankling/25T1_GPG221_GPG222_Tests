using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToToggle;

    public void ToggleObject()
    {
        if (objectToToggle.activeSelf)
        {
            objectToToggle.SetActive(false);
        }
        else
        {
            objectToToggle.SetActive(true);
        }
    }
}
