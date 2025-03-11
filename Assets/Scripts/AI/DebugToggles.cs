using UnityEngine;

public class DebugToggles : MonoBehaviour
{
    public static bool DrawRays;
    public static bool DrawCalculatedPaths;
    public static bool DrawTargetRoutes;
    public static bool DrawNeighbourSphere;
    public static bool DrawNeighbourRays;
    public static bool DrawAlignRays;

    public void ToggleDrawRays(bool drawRays)
    {
        DrawRays = drawRays;
    }
    
    public void ToggleDrawCalculatedPaths(bool drawCalculatedPaths)
    {
        DrawCalculatedPaths = drawCalculatedPaths;
    }

    public void ToggleDrawTargetRoutes(bool drawTargetRoutes)
    {
        DrawTargetRoutes = drawTargetRoutes;
    }

    public void ToggleDrawNeighbourSphere(bool drawNeighbourSphere)
    {
        DrawNeighbourSphere = drawNeighbourSphere;
    }

    public void ToggleDrawNeighbourRays(bool drawNeighbourRays)
    {
        DrawNeighbourRays = drawNeighbourRays;
    }

    public void ToggleDrawAlignRays(bool drawAlignRays)
    {
        DrawAlignRays = drawAlignRays;
    }

    public void ToggleMoveForwards(bool moveForwardsToggle)
    {
        MoveForward[] array = FindObjectsByType<MoveForward>(FindObjectsSortMode.None);

        foreach (MoveForward item in array)
        {
            item.enabled = moveForwardsToggle;
        }
    }

    public void ToggleWander(bool wanderToggle)
    {
        Wander[] array = FindObjectsByType<Wander>(FindObjectsSortMode.None);

        foreach (Wander item in array)
        {
            item.enabled = wanderToggle;
        }
    }

    public void ToggleAvoid(bool avoidToggle)
    {
        Avoid[] array = FindObjectsByType<Avoid>(FindObjectsSortMode.None);

        foreach (Avoid item in array)
        {
            item.enabled = avoidToggle;
        }
    }

    public void ToggleAlign(bool alignToggle)
    {
        Align[] array = FindObjectsByType<Align>(FindObjectsSortMode.None);

        foreach (Align item in array)
        {
            item.enabled = alignToggle;
        }
    }

    public void ToggleSeparate(bool separateToggle)
    {
        Separate[] array = FindObjectsByType<Separate>(FindObjectsSortMode.None);

        foreach (Separate item in array)
        {
            item.enabled = separateToggle;
        }
    }
}
