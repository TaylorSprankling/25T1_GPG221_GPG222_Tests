using UnityEngine;

public class BehaviourControllerCustomPather : BehaviourController
{
    [SerializeField] private CustomPathFollower customPathFollower;
    [SerializeField] private bool stopOnTargetReached = true;
    
}
