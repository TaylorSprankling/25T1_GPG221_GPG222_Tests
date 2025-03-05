using UnityEngine;

public class DebugToggles : MonoBehaviour
{
    public static bool DrawRays;
    public static bool DrawCalculatedPaths;
    public static bool DrawTargetRoutes;

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
}
